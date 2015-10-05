using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoggerApp.Api;
using LoggerApp.NLogImpl;
using LoggerApp.SerilogImpl;

namespace LoggerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DoStuff(new SerilogLogger());
        }

        public static void DoStuff(ILogger logger)
        {
            Console.WriteLine($"doing stuff with {logger.GetType().Name}");
            var jobFactory = new JobFactory(logger);

            int count = 50000;
            int completed = 0;
            double errorRate = 0.01;

            var cancellation = new CancellationTokenSource();

            Task.Run(async () =>
            {
                while (!cancellation.IsCancellationRequested)
                {
                    Console.WriteLine($"Completed {completed}/{count} jobs");
                    await Task.Delay(1000, cancellation.Token);
                }
            }, cancellation.Token);

            foreach (var job in jobFactory.GetJobsForExecution(count).AsParallel())
            {
                job.Execute(errorRate);
                Interlocked.Increment(ref completed);
            }

            cancellation.Cancel();
        }
    }
}
