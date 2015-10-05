using System;
using System.Linq;
using System.Threading;

namespace LoggerApp.Api
{
    internal class Job
    {
        static readonly Random Random = new Random();

        private readonly ILogger _logger;

        public Job(ILogger logger)
        {
            _logger = logger;
        }

        public int JobId { get; set; }

        public void Execute(double errorRate)
        {
            _logger.Info("Starting job execution");

            try
            {
                ExecuteInternal(errorRate);
                _logger.Info("Job execution completed");
            }
            catch (Exception ex)
            {
                _logger.Info(ex, "Executing job failed!");
            }
        }

        private void ExecuteInternal(double errorRate)
        {
            Thread.Sleep(Random.Next(0, 5));

            if (Random.Next(0, (int) (1 / errorRate)) == 0)
            {
                throw new Exception("Job failed because something died on us!");
            }

            foreach (var citizen in CitizenContainer.GetCitizens(100).AsParallel())
            {
                ProcessCitizen(citizen);
            }

            _logger.Info("Job ExecuteInternal went well");
        }

        private void ProcessCitizen(Citizen citizen)
        {
            _logger.Info("Processing citizen started");

            //todo: some businesslogic???

            _logger.Info("Processing citizen ended");
        }
    }
}