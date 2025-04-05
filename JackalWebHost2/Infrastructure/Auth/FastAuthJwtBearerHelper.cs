using System.IdentityModel.Tokens.Jwt;
using JackalWebHost2.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace JackalWebHost2.Infrastructure.Auth
{
    public static class FastAuthJwtBearerHelper
    {
        public static User ExtractUser(ClaimsPrincipal? user)
        {
            var idClaim = user?.FindFirst(AuthDefaults.FastAuthUserId);
            var loginClaim = user?.FindFirst(AuthDefaults.FastAuthLogin);

            return new User
            {
                Id = long.TryParse(idClaim?.Value, out var id) ? id : 0,
                Login = loginClaim?.Value ?? "dark incognito"
            };
        }

        public static async Task<string> SignInUser(HttpContext httpContext, User user)
        {
            var jwt = new JwtSecurityToken(
                issuer: AuthDefaults.Issuer,
                audience: AuthDefaults.Audience,
                claims: new List<Claim>
                {
                    new Claim(AuthDefaults.FastAuthUserId, user.Id.ToString()),
                    new Claim(AuthDefaults.FastAuthLogin, user.Login)
                },
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(365)),
                signingCredentials: new SigningCredentials(AuthDefaults.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

    }
}
