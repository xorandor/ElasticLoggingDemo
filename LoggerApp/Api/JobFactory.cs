using System.Collections.Generic;
using System.Linq;

namespace LoggerApp.Api
{
    internal class JobFactory
    {
        private readonly ILogger _logger;

        public JobFactory(ILogger logger)
        {
            _logger = logger;
        }

        public IEnumerable<Job> GetJobsForExecution(int count)
        {
            var lastJobId = Settings.GetSetting<int>("lastjobid");

            var result = Enumerable.Range(lastJobId + 1, count).Select(x => new Job(_logger)
            {
                JobId = x
            });

            Settings.SetSetting("lastjobid", result.Max(x => x.JobId));
            return result;
        }
    }
}