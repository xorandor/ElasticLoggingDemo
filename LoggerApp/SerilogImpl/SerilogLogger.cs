using System;
using System.Diagnostics;
using System.IO;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using ILogger = LoggerApp.Api.ILogger;

namespace LoggerApp.SerilogImpl
{
    class SerilogLogger : ILogger
    {
        private readonly Serilog.ILogger _logger;
        public SerilogLogger()
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                //.WriteTo.ColoredConsole()
                .WriteTo.Sink<MySink>()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                {
                    AutoRegisterTemplate = true,
                })
                .Enrich.WithMachineName()
                .Enrich.WithProperty("processId", Process.GetCurrentProcess().Id)
                .Enrich.FromLogContext()
                .CreateLogger();
            
            _logger = logger;
        }

        public void Info(string message)
        {
            _logger.Information(message);
        }

        public void Info(Exception exception, string message)
        {
            _logger.Information(exception, message);
        }
    }

    internal class MySink : ILogEventSink
    {
        public void Emit(LogEvent logEvent)
        {
            var text = logEvent.RenderMessage();

            using (var writer = new StringWriter())
            {
                new ElasticsearchJsonFormatter().Format(logEvent, writer);
                var str = writer.ToString();
                //Console.WriteLine(str);
            }
        }
    }

}
