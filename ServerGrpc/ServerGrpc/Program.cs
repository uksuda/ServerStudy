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

            var builder = WebApplication.CreateBuilder(args);

            // Additional configuration is required to successfully run gRPC on macOS.
            // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

            builder.Host.ConfigureLogging(logging =>
            {
                logging.ClearProviders()
                    .AddSerilog(Log.Logger);
            });

            // Add services to the container.
            builder.Services.AddGrpc(options =>
            {
                options.Interceptors.Add<ServerInterceptor>();
            });

            builder.Services.AddSingleton<MainService>();

            var app = builder.Build();

            //app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseEndpoints(endPoints =>
            {
                endPoints.MapGrpcService<MainController>();
            });

            // Configure the HTTP request pipeline.
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            Log.Information($"Env: {app.Environment.ApplicationName}, {app.Environment.EnvironmentName}");
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
    }
}