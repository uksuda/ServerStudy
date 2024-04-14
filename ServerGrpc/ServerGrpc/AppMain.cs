using Microsoft.AspNetCore.Authentication.JwtBearer;
using MySqlConnector.Logging;
using ServerGrpc.Common;
using ServerGrpc.Controller;
using ServerGrpc.DB;
using ServerGrpc.Grpc;
using ServerGrpc.Infra;
using ServerGrpc.Logger;
using ServerGrpc.Services;
using System.Reflection;

namespace ServerGrpc
{
    public class AppMain
    {
        private readonly CancellationTokenSource _tokenSource;
        private readonly ILogger _logger;
        private readonly string[] _args;

        public AppMain(ILogger logger, string[] args)
        {
            _tokenSource = new CancellationTokenSource();

            _logger = logger;
            _args = args;
        }

        public void Run()
        {
            var app = CreateHostBuilder(_logger, _args).Build();
            var lifeTime = app.Services.GetRequiredService<IHostApplicationLifetime>();
            
            //var tokenSource = app.Services.GetRequiredService<CancellationTokenSource>();
            //lifeTime.ApplicationStarted;
            //lifeTime.ApplicationStopping;
            //lifeTime.ApplicationStarted;

            app.Run();

            //AppDomain.CurrentDomain.ProcessExit += (s, e) =>
            //{
            //    logger.LogDebug("End Program");
            //};
        }

        public IHostBuilder CreateHostBuilder(ILogger logger, string[] args)
        {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((ctx, config) =>
                {
                    if (ctx.HostingEnvironment.IsEnvironment("development") || OperatingSystem.IsWindows())
                    {
                        config.AddJsonFile("appsettings.development.json");
                        _logger.LogInformation("App start: development env");
                    }
                    else
                    {
                        config.AddJsonFile("appsettings.json");
                        _logger.LogInformation("App start: production env");
                    }
                })
                .ConfigureLogging((ctx, builder) =>
                {
                    builder.ClearProviders();
                    builder.AddProvider(AppLogManager.GetProvider());
                })
                .ConfigureWebHostDefaults(builder =>
                {
                    ConfigureServices(builder);
                    Configure(builder);
                });

            return builder;
        }

        public void ConfigureServices(IWebHostBuilder builder)
        {
            builder.ConfigureServices((ctx, services) =>
            {
                var isDevelop = (ctx.HostingEnvironment.IsEnvironment("development") || OperatingSystem.IsWindows());
                var appsettings = ctx.Configuration.Get<AppSettings>();
                services.AddSingleton(appsettings);

                var jwtBuilder = new JwtTokenBuilder(appsettings);
                services.AddSingleton(jwtBuilder);

                services.AddAuthorization(options =>
                {
                    options.AddPolicy(ServerPolicy.Admin, ServerPolicy.CreatePolicyAdmin());
                    options.AddPolicy(ServerPolicy.User, ServerPolicy.CreatePolicyUser());
                })
                .AddCors(x => 
                { 
                    x.AddPolicy("AllowAll", builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                        .WithExposedHeaders(
                            "Grpc-Status", 
                            "Grpc-Message", 
                            "Grpc-Encoding", 
                            "Grpc-Accept-Encoding");
                    }); 
                })
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = jwtBuilder.TokenValidationParameter;

                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    //options.TokenValidationParameters = new TokenValidationParameters
                    //{
                    //    ValidateIssuer = true,
                    //    ValidateAudience = true,
                    //    ValidateIssuerSigningKey = true,
                    //    ValidAudience = appsettings.Jwt.Audience,
                    //    ValidIssuer = appsettings.Jwt.Issuer,
                    //    ValidateLifetime = false,
                    //    ClockSkew = TimeSpan.Zero,
                    //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appsettings.Jwt.Secret)),
                    //};
                    options.Events = new JwtBearerEvents();
                    options.Events.OnTokenValidated = (context) =>
                    {
                        return Task.CompletedTask;
                    };
                    options.Events.OnMessageReceived = (context) =>
                    {
                        return Task.CompletedTask;
                    };
                });

                MySqlConnectorLogManager.Provider = new SerilogLoggerProvider();

                services.AddGrpc(option =>
                {
                    option.MaxReceiveMessageSize = ServerConst.GRPC_MAX_RECV_SIZE;
                    option.MaxSendMessageSize = ServerConst.GRPC_MAX_SEND_SIZE;
                    option.ResponseCompressionAlgorithm = ServerConst.GRPC_COMPRESS_ALGORITHM;

                    if (isDevelop == true)
                    {
                        option.EnableDetailedErrors = true;
                        services.AddGrpcReflection();
                    }
                    option.Interceptors.Add<AutoHeaderInterceptor>();
                    option.Interceptors.Add<ServerInterceptor>();
                });

                if (isDevelop == true)
                {
                    services.AddGrpcReflection();
                }

                services.AddControllers().AddApplicationPart(Assembly.GetExecutingAssembly());

                // cancel token
                //services.AddSingleton<CancellationTokenSource>();
                services.AddSingleton(_tokenSource);

                // service
                services.AddSingleton<MainService>();

                // context
                services.AddSingleton<AccountDBContext>();
                services.AddSingleton<GameDBContext>();
                services.AddSingleton<LogDBContext>();

                services.AddSingleton<AccountRedisContext>();
                services.AddSingleton<GameRedisContext>();

                services.AddSingleton<ClientManager>();
            });
        }
        public void Configure(IWebHostBuilder builder)
        {
            builder.Configure((ctx, app) =>
            {
                app.UseRouting();
                app.UseCors();
                app.UseAuthentication();
                app.UseAuthorization();

                app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

                app.UseEndpoints(endpoints =>
                {
                    if (ctx.HostingEnvironment.IsDevelopment() || OperatingSystem.IsWindows())
                    {
                        endpoints.MapGrpcReflectionService();
                    }
                    endpoints.MapControllers();

                    //endpoints.MapGrpcService<MainController>().EnableGrpcWeb().RequireCors("AllowAll");
                    endpoints.MapGrpcService<MainController>();

                    //endpoints.MapGet(
                    //    "/",
                    //    () =>
                    //        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909"
                    //);
                });

                var endpointUrls = ctx.Configuration.GetValue<string>("Kestrel:Endpoints:Http:Url");
                _logger.LogInformation($"Urls: {endpointUrls}");
            });
        }
    }
}
