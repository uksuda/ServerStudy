using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using ServerGrpc.Controller;
using ServerGrpc.Grpc;
using ServerGrpc.Infra;
using ServerGrpc.Logger;
using ServerGrpc.Services;
using System.Text;

namespace ServerGrpc
{
    public class StartUp
    {
        private readonly ILogger _logger = AppLogManager.GetLogger<StartUp>();
        private readonly IConfiguration _config;

        public StartUp(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            bool isDevelop = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("development");
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
                        //OnTokenValidated = (ctx) =>
                        //{
                        //    // TODO
                        //    return Task.CompletedTask;
                        //},
                        OnTokenValidated = OnTokenValidate,
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

            services.AddGrpc(options =>
            {
                options.MaxSendMessageSize = ServerConst.GRPC_MAX_SEND_SIZE;
                options.MaxReceiveMessageSize = ServerConst.GRPC_MAX_RECV_SIZE;
                options.ResponseCompressionAlgorithm = ServerConst.GRPC_COMPRESS_ALGORITHM;

                options.Interceptors.Add<AutoHeaderInterceptor>();
                options.Interceptors.Add<ServerInterceptor>();

                if (isDevelop)
                {
                    options.EnableDetailedErrors = true;
                    //services.AddGrpcReflection();
                }
            });

            services.AddGrpcReflection();

            services.AddControllers().AddApplicationPart(System.Reflection.Assembly.GetExecutingAssembly());

            //
            services.AddSingleton<JwtTokenBuilder>();
            services.AddSingleton<MainService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                
            }
            
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

            app.UseEndpoints(endPoints =>
            {
                if (env.IsDevelopment() == true)
                {
                    endPoints.MapGrpcReflectionService();
                }
                endPoints.MapControllers();

                endPoints.MapGrpcService<MainController>();
            });

            // Configure the HTTP request pipeline.
            //app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
            var endpointUrl = _config.GetValue<string>("Kestrel:Endpoints:http:Url");

        }

        public async Task OnTokenValidate(TokenValidatedContext ctx)
        {
            //if (ctx.SecurityToken is JwtSecurityToken token)
            //if (ctx.SecurityToken is JsonWebToken token)
            //{
            //    var tokenString = token.UnsafeToString();
            //    var jwtBuilder = ctx.HttpContext.RequestServices.GetRequiredService<JwtTokenBuilder>();
            //    var (prins, validToken) = jwtBuilder.ValidateToken(tokenString);
            //    //var (prins, token) = tokenBuilder.ValidateToken(token.ToString());
            //}
            await Task.CompletedTask;
        }
    }
}
