using Serilog;
using ServerGrpc.Controller;
using ServerGrpc.Grpc;
using ServerGrpc.Logger;
using ServerGrpc.Services;
using System.Net;

namespace ServerGrpc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppLogManager.Init();

            Log.Information("Start ");

            var builder = CreateHostBuilder(args)
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

            var app = builder.Build();

            //app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            //app.Run();

            // Additional configuration is required to successfully run gRPC on macOS.
            // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
            
            //Log.Information($"Env: {app.Environment.ApplicationName}, {app.Environment.EnvironmentName}");
            Log.Information("app started");

            try
            {
                app.Run();
            }
            catch (Exception e)
            {
                Log.Error($"{e}");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseStartup<StartUp>();
                });
        }
    }
}