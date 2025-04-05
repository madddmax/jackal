using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JackalWebHost2.Infrastructure.Auth;

public static class AuthDefaults
{
    public const string FastAuthScheme = "FastAuthScheme";
    public const string FastAuthPolicy = "FastAuthPolicy";
    public const string FastAuthUserId = "UserId";
    public const string FastAuthLogin = "Login";

    public const string Issuer = "jackal.team";
    public const string Audience = "jackal.team";
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecKey));

    const string SecKey = "otoqPRQij8WUxi0C7YDdMEiT6Xh9dWczyFShVmPYcLZvNewFY7n4Nh68A/X8MbCB";
}