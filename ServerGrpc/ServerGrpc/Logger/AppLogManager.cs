using Serilog;
using Serilog.Extensions.Logging;

namespace ServerGrpc.Logger
{
    public class AppLogManager
    {
        //private static Serilog.ILogger _logger;
        private static ILoggerProvider _provider;

        public static void Init()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "log\\app_.log";
            Log.Logger = new LoggerConfiguration()
                //.MinimumLevel.Debug()
                //.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                //.MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
                //.MinimumLevel.Override("Default", Serilog.Events.LogEventLevel.Fatal)
                .WriteTo.Console()
                .WriteTo.File(path, rollingInterval: RollingInterval.Day)
                .CreateBootstrapLogger();

            _provider = new SerilogLoggerProvider(Log.Logger);
        }

        public static ILoggerProvider GetProvider()
        {
            if (_provider == null)
            {
                Init();
            }
            return _provider;
        }

        public static Microsoft.Extensions.Logging.ILogger GetLogger<T>()
        {
            if (_provider == null)
            {
                Init();
            }
            return GetLogger(typeof(T));
        }

        public static Microsoft.Extensions.Logging.ILogger GetLogger(Type type)
        {
            if (_provider == null)
            {
                Init();
            }
            return GetLogger(type.Name);
        }

        public static Microsoft.Extensions.Logging.ILogger GetLogger(string name)
        {
            if (_provider == null)
            {
                Init();
            }
            return _provider.CreateLogger(name);
        }
    }
}
