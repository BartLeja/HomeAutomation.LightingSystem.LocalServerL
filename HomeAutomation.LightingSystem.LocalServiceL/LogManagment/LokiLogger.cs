using Serilog;
using Serilog.Sinks.Loki;

namespace HomeAutomation.LightingSystem.LocalServiceL.LogManagment
{
    public class LokiLogger : ILogger
    {
        private NoAuthCredentials credentials;
      
        public LokiLogger()
        {
             credentials = new NoAuthCredentials("http://localhost:3100");
             Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.LokiHttp(credentials)
                .CreateLogger();
        }

        public void LogInformation(string information)
        {
            Log.Information(information);
           // Log.F
        }
    }
}
