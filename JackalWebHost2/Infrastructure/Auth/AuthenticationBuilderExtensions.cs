using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace JackalWebHost2.Infrastructure.Auth;

public static class AuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddFastAuthCookie(
        this AuthenticationBuilder builder,
        string authenticationScheme,
        Action<CookieAuthenticationOptions> configureOptions)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<CookieAuthenticationOptions>, PostConfigureCookieAuthenticationOptions>());
        builder.Services.AddOptions<CookieAuthenticationOptions>(authenticationScheme).Validate(o => !o.Cookie.Expiration.HasValue, "Cookie.Expiration is ignored, use ExpireTimeSpan instead.");
        return builder.AddScheme<CookieAuthenticationOptions, FastAuthAuthenticationHandler>(authenticationScheme, null, configureOptions);
    }
}