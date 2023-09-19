using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace ServerGrpc.Infra
{
    public static class ServerPolicy
    {
        public const string User = "User";
        public const string Admin = "Admin";

        public static AuthorizationPolicy CreatePolicyUser()
        {
            var builder = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireRole(User);
                
            return builder.Build();
        }

        public static AuthorizationPolicy CreatePolicyAdmin()
        {
            var builder = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireRole(Admin);

            return builder.Build();
        }
    }
}
