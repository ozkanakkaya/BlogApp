using BlogApp.Core.DTOs.Concrete;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogApp.API.Jwt
{
    public class TokenGenerator
    {
        public static TokenResponse GenerateToken(CheckUserResponseDto dto, List<RoleDto> roles)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenSettings.Key));

            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>();

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Definition));
            }

            claims.Add(new Claim(ClaimTypes.Name, dto.Username));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, dto.Id.ToString()));

            var expireDate = DateTime.UtcNow.AddMinutes(TokenSettings.Expire);

            JwtSecurityToken token = new JwtSecurityToken(issuer: TokenSettings.Issuer, audience: TokenSettings.Audience, claims: claims, notBefore: DateTime.UtcNow, expires: expireDate, signingCredentials: credentials);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            return new TokenResponse(handler.WriteToken(token), expireDate);
        }
    }
}
