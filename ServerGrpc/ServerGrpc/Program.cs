using Serilog;
using ServerGrpc.Controller;
using ServerGrpc.Services;

namespace ServerGrpc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "log\\app_.log";
            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File(path, rollingInterval: RollingInterval.Day)
                .CreateBootstrapLogger();

            Log.Information("Start ");

            var builder = WebApplication.CreateBuilder(args);

            // Additional configuration is required to successfully run gRPC on macOS.
            // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

            
            
            builder.Host.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                //logging.AddSerilog(Log.Logger);
                //logging.AddConsole();
            })
            .UseSerilog((ctx, lc) =>
            {
                lc.MinimumLevel.Debug()
                  .WriteTo.Console()
                  .WriteTo.File(path, rollingInterval: RollingInterval.Day);
            });

            // Add services to the container.
            builder.Services.AddGrpc();

            builder.Services.AddSingleton<MainService>();

            var app = builder.Build();

            var test = app.Services.GetRequiredService<MainService>();

            //app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseEndpoints(endPoints =>
            {
                endPoints.MapGrpcService<MainController>();
            });

            // Configure the HTTP request pipeline.
            //app.MapGrpcService<AccountService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            Log.Warning("app started");
            app.Run();
        }
    }
}