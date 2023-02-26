using Serilog;

namespace ServerGrpc.Logger
{
    public class AppLogManager
    {
        private static Serilog.ILogger _logger;

        public static void Init()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "log\\app_.log";
            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Fatal)
                .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Fatal)
                //.MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Fatal)
                //.MinimumLevel.Override("Default", Serilog.Events.LogEventLevel.Fatal)
                .WriteTo.Console()
                .WriteTo.File(path, rollingInterval: RollingInterval.Day)
                .CreateBootstrapLogger();

            _logger = Log.Logger;
        }

        public static Serilog.ILogger GetLogger()
        {
            return _logger;
        }
    }
}
