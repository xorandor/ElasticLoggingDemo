using System;

namespace LoggerApp.Api
{
    public interface ILogger
    {
        void Info(string message);
        void Info(Exception exception, string message);
    }
}
