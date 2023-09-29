using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using ServerGrpc.Controller;
using ServerGrpc.Grpc;
using ServerGrpc.Infra;
using ServerGrpc.Services;
using System.Text;

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
                option.AddPolicy(ServerPolicy.Admin, ServerPolicy.CreatePolicyAdmin());
                option.AddPolicy(ServerPolicy.User, ServerPolicy.CreatePolicyAdmin());
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = "Audience", // TODO
                        ValidIssuer = "Issuer", // TODO
                        ValidateLifetime = false,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtTokenBuilder.Secret)),
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = (ctx) =>
                        {
                            // TODO
                            return Task.CompletedTask;
                        },
                        OnMessageReceived = (ctx) =>
                        {
                            // TODO
                            return Task.CompletedTask;
                        }                        
                    };
                });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", (builder) =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                    .WithExposedHeaders(
                        "Grpc-Status",
                        "Grpc-Message",
                        "Grpc-Encoding",
                        "Grpc-Accept-Encoding");
                });
            });

            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddSerilog(Log.Logger);
            });

            services.AddGrpc(options =>
            {
                options.MaxSendMessageSize = 1024 * 1024 * 4;
                options.MaxReceiveMessageSize = 1024 * 1024 * 4;
                options.Interceptors.Add<ServerInterceptor>();

                //if (development)
                //{
                //    options.EnableDetailedErrors = true;
                //}
            });
            
            //
            services.AddSingleton<MainService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {

            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseGrpcWeb();
            app.UseEndpoints(endPoints =>
            {
                endPoints.MapGrpcService<MainController>();
            });

            // Configure the HTTP request pipeline.
            //app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
        }
    }
}
