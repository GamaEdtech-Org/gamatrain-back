namespace GamaEdtech.Common.Identity.ApiKey
{
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using System.Text.Encodings.Web;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, IConfiguration configuration)
        : AuthenticationHandler<ApiKeyAuthenticationSchemeOptions>(options, logger, encoder)
    {
        private const string BearerPrefix = "ApiKey ";

        public static string? GetTokenFromHeader([NotNull] HttpRequest httpRequest)
        {
            var authorization = httpRequest.Headers.Authorization.ToString();
            return authorization.StartsWith(BearerPrefix, StringComparison.OrdinalIgnoreCase)
                ? authorization[BearerPrefix.Length..]
                : null;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var token = GetTokenFromHeader(Context.Request);
            if (token is null)
            {
                return AuthenticateResult.NoResult();
            }

            if (configuration.GetValue<string?>("ApiKey") != token)
            {
                return AuthenticateResult.Fail("Invalid ApiKey");
            }

            List<Claim> lst = [new(PermissionConstants.ApiKeyPolicy, token)];
            var principal = new ClaimsPrincipal(new ClaimsIdentity(lst, Scheme.Name));
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return await Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
