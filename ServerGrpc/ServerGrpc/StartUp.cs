using Microsoft.AspNetCore.Authorization;
using Serilog;
using ServerGrpc.Controller;
using ServerGrpc.Grpc;
using ServerGrpc.Services;

namespace ServerGrpc
{
    public class StartUp
    {
        public IConfiguration Config { get; }
        public StartUp(IConfiguration config)
        {
            Config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization(option =>
            {
                //option.AddPolicy()
            });

            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddSerilog(Log.Logger);
            });

            services.AddGrpc(options =>
            {
                options.Interceptors.Add<ServerInterceptor>();
            });

            services.AddSingleton<MainService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {

            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endPoints =>
            {
                endPoints.MapGrpcService<MainController>();
            });

            // Configure the HTTP request pipeline.
            //app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
        }
    }
}
