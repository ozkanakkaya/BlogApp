using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Response;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogApp.Business.Jwt
{
    public class TokenGenerator
    {
        private readonly TokenSettings _settings;

        public TokenGenerator(IOptionsSnapshot<TokenSettings> settings)
        {
            _settings = settings.Value;
        }

        public TokenResponse GenerateToken(CheckUserResponseDto dto, RoleListDto roles)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));

            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>();

            foreach (var role in roles.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            claims.Add(new Claim(ClaimTypes.Name, dto.Username));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, dto.Id.ToString()));

            var expireDate = DateTime.UtcNow.AddMinutes(_settings.Expire);

            JwtSecurityToken token = new JwtSecurityToken(issuer: _settings.Issuer, audience: _settings.Audience, claims: claims, notBefore: DateTime.UtcNow, expires: expireDate, signingCredentials: credentials);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            return new TokenResponse(handler.WriteToken(token), expireDate);
        }
    }
}
