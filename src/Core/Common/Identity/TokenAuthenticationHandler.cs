namespace GamaEdtech.Common.Identity
{
    using System;
    using System.Security.Claims;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using static GamaEdtech.Common.Core.Constants;

    public class TokenAuthenticationHandler(IOptionsMonitor<TokenAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
        : AuthenticationHandler<TokenAuthenticationSchemeOptions>(options, logger, encoder)
    {
        private const string BearerPrefix = "Bearer ";

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authorization = Context.Request.Headers.Authorization.ToString();
            if (!authorization.StartsWith(BearerPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.NoResult();
            }

            var data = authorization[BearerPrefix.Length..].Split(DelimiterAlternate, 2, StringSplitOptions.RemoveEmptyEntries);
            if (data.Length != 2)
            {
                return AuthenticateResult.NoResult();
            }

            var identityService = Context.RequestServices.GetRequiredService<ITokenService>();
            var result = await identityService.VerifyTokenAsync(new VerifyTokenRequest
            {
                Token = data[1],
                UserId = data[0],
                TokenProvider = PermissionConstants.ApiDataProtectorTokenProvider,
                Purpose = PermissionConstants.ApiDataProtectorTokenProviderAccessToken,
            });
            if (result?.Claims is null)
            {
                return AuthenticateResult.NoResult();
            }

            var principal = new ClaimsPrincipal(new ClaimsIdentity(result.Claims, Scheme.Name));
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
