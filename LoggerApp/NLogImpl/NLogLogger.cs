using System;
using System.Reflection;
using System.Text;
using NLog;
using NLog.Config;
using NLog.LayoutRenderers;
using NLog.Targets;
using NLog.Targets.ElasticSearch;
using NLog.Targets.Wrappers;

namespace LoggerApp.NLogImpl
{
    class NLogLogger : Api.ILogger
    {
        readonly Logger _logger =  LogManager.GetLogger("loggerApp");
        public NLogLogger()
        {
            ConfigurationItemFactory.Default.RegisterItemsFromAssembly(Assembly.GetExecutingAssembly());

            var config = new LoggingConfiguration();

            //var consoleTarget = new ColoredConsoleTarget();
            //config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, consoleTarget));
            
            var elasticTarget = new BufferingTargetWrapper(new ElasticSearchTarget
            {
                Fields =
                {
                    new ElasticSearchField
                    {
                        Name = "application",
                        Layout = "LoggerAppNLog"
                    },
                    new ElasticSearchField
                    {
                        Name = "machinename",
                        Layout = "${machinename}"
                    },
                }
            }, 1000, 1000);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, elasticTarget));

            LogManager.Configuration = config;
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Info(Exception exception, string message)
        {
            _logger.Info(exception, message);
        }
    }

    [LayoutRenderer("CorrelationId")]
    public class CorrelationIdRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(logEvent.Properties["CorrelationId"]);
        }
    }
}
