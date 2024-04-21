using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using ServerGrpc.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServerGrpc.Infra
{
    public class JwtTokenBuilder
    {
        public const string GUID = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti;
        public const string MBER_NO = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.NameId;

        private const int ADMIN_TOKEN_EXPIRE_SEC = 60 * 5; // 5분
        private const int USER_TOKEN_EXPIRE_SEC = 60 * 5;

        private readonly string _issuer;
        private readonly string _audience;

        private readonly SigningCredentials _signCredentials;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly SecurityTokenDescriptor _tokenDescriptor;


        public TokenValidationParameters TokenValidationParameter => _tokenValidationParameters;

        public JwtTokenBuilder(AppSettings appsettings)
        {
            var secret = Encoding.ASCII.GetBytes(appsettings.Jwt.Secret);
            _issuer = appsettings.Jwt.Issuer;
            _audience = appsettings.Jwt.Audience;

            var key = new SymmetricSecurityKey(secret);
            _signCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            _tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = appsettings.Jwt.Audience,
                ValidIssuer = appsettings.Jwt.Issuer,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = key,
            };
            _tokenHandler = new JwtSecurityTokenHandler();

            _tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = _signCredentials,
                //Subject,
                //Expires,
            };
        }

        public string GenerateToken(string guid, int mberNo, string role = ServerPolicy.User)
        {
            bool isAdmin = role.Equals(ServerPolicy.Admin);

            ClaimsIdentity claimsIdentity = (isAdmin) ? GetAdminClaims(guid, mberNo) : GetUserClaims(guid, mberNo);
            int expire = (isAdmin) ? ADMIN_TOKEN_EXPIRE_SEC : USER_TOKEN_EXPIRE_SEC;
            return BuildToken(DateTime.UtcNow, expire, claimsIdentity);
        }

        public (ClaimsPrincipal, JsonWebToken) ValidateToken(string token)
        {
            var principal = _tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
            return (principal, validatedToken as JsonWebToken);
        }

        private ClaimsIdentity GetAdminClaims(string guid, int mberNo)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, ServerPolicy.Admin),
                new Claim(GUID, guid),
                new Claim(MBER_NO, mberNo.ToString()),
            };
            return new ClaimsIdentity(claims);
        }

        private ClaimsIdentity GetUserClaims(string guid, int mberNo)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, ServerPolicy.User),
                new Claim(GUID, guid),
                new Claim(MBER_NO, mberNo.ToString()),
            };
            return new ClaimsIdentity(claims);
        }

        private string BuildToken(DateTime now, int expireSec, ClaimsIdentity claimsIdentity)
        {
            _tokenDescriptor.Subject = claimsIdentity;
            _tokenDescriptor.Expires = now.AddSeconds(expireSec);

            var token = _tokenHandler.CreateToken(_tokenDescriptor);
            return token.UnsafeToString();
        }
    }
}
