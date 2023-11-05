using ServerGrpc.Logger;
using System.Net;
using System.Reflection;
using System.Text;

namespace ServerGrpc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppLogManager.Init();

            var logger = AppLogManager.GetLogger("Program");
            
            PrintInfomation(logger);

            var builder = CreateHostBuilder(args);
            var app = builder.Build();

            //app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            //app.Run();

            // Additional configuration is required to successfully run gRPC on macOS.
            // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

            //Log.Information($"Env: {app.Environment.ApplicationName}, {app.Environment.EnvironmentName}");

            logger.LogInformation("server started");

            try
            {
                app.Run();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseStartup<StartUp>();
                })
                .ConfigureLogging((ctx, builder) =>
                {
                    builder.ClearProviders();
                    builder.AddProvider(AppLogManager.GetProvider());
                })
                .ConfigureAppConfiguration((ctx, config) =>
                {
                    if (ctx.HostingEnvironment.IsDevelopment() == true)
                    {
                        config.AddJsonFile("appsettings.Development.json");
                    }
                    else
                    {
                        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                        config.AddJsonFile($"appsettings.{env}.json");
                    }
                });
        }

        public static void PrintInfomation(ILogger logger)
        {
            logger.LogInformation("Name: [{0}]", Assembly.GetEntryAssembly().FullName);
            
            var host = Dns.GetHostName();
            var ipEntry = Dns.GetHostEntry(host);

            logger.LogInformation("Host: [{0}]", host);

            foreach (var ip in ipEntry.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    logger.LogInformation("Endpoint: {0}", ip);
                }
            }
        }
    }
}