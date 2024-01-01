using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServerGrpc.Infra
{
    public class JwtTokenBuilder
    {
        public const string Audience = "Client";
        public const string Issuer = "Server";
        public const string Secret = "Secret"; // qk77n79is707yy9s7gsgr7wxlksef7km

        // claims
        public const string JwtUserUid = "uid";

        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly TokenValidationParameters _validateParameters;
        private readonly SigningCredentials _signingCredentials;

        private readonly ILogger<JwtTokenBuilder> _logger;

        public JwtTokenBuilder(ILogger<JwtTokenBuilder> logger)
        {
            var secret = Encoding.ASCII.GetBytes(Secret);
            _signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature);
            _validateParameters = new TokenValidationParameters
            {
                ValidIssuer = Issuer,
                ValidAudience = Audience,
                ValidateIssuer = true,
                ValidateAudience = true,
                IssuerSigningKey = new SymmetricSecurityKey(secret),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero,
            };
            _tokenHandler = new JwtSecurityTokenHandler();

            _logger = logger;
        }

        public string GenerateJwtToken(DateTime notBefore, int expireSec, string role, string jti, string uid)
        {
            var claims = GetClaims(role, jti, uid);
            
            ClaimsIdentity identity = new ClaimsIdentity(claims);

            var jwtToken = _tokenHandler.CreateJwtSecurityToken(Issuer, Audience, identity, notBefore, DateTime.Now.AddSeconds(expireSec), signingCredentials: _signingCredentials);
            return _tokenHandler.WriteToken(jwtToken);
        }

        public Claim[] GetClaims(string role, string jti, string uid)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, jti),
                new Claim(JwtUserUid, uid),
            };
            return claims;
        }
        
        public (ClaimsPrincipal, JwtSecurityToken) ValidateToken(string token)
        {
            //var jwtToken = _tokenHandler.ReadJwtToken(token);
            var principal = _tokenHandler.ValidateToken(token, _validateParameters, out var validatedToken);
            return (principal, validatedToken as JwtSecurityToken);
        }
    }
}
