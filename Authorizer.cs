using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthorizer {

public static class Authorizer {

    public static bool IsAuthorized(string authHeaderValue, string domain, string audience, List<string> scopes) {
        try {
            var toks = authHeaderValue.Split();
            var domainUrl = $"https://{domain}/";
            ConfigurationManager<OpenIdConnectConfiguration> configManager = 
                new ConfigurationManager<OpenIdConnectConfiguration>(
                    $"{domainUrl}.well-known/openid-configuration", 
                    new OpenIdConnectConfigurationRetriever());
            OpenIdConnectConfiguration openIdConfig = 
            AsyncHelper.RunSync(async () => await configManager.GetConfigurationAsync(CancellationToken.None));
            TokenValidationParameters validationParameters =
            new TokenValidationParameters
            {
                ValidIssuer = domainUrl,
                ValidAudiences = new[] { audience },
                IssuerSigningKeys = openIdConfig.SigningKeys,

            };

            SecurityToken validatedToken;
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var user = handler.ValidateToken(toks[1], validationParameters, out validatedToken);

            // Check for permissions if scopes are provided
            var permissions = user.Claims.Where(c=>c.Type == "permissions").First().Value;
            if (scopes != null && scopes.Count > 0 && permissions != null) {
                if (scopes.Any(sc=>permissions.Contains(sc))) {
                    return true;
                }
                return false;
            } 
            return true;

        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }

    }

    internal static class AsyncHelper
    {
        private static readonly TaskFactory TaskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

        public static void RunSync(Func<Task> func)
        {
            TaskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
        }

        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return TaskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
        }
    }

}



}