using System;
using System.Globalization;
using IdentityModel = System.IdentityModel.Tokens;
using System.Xml;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;

namespace RecordLion.RecordsManager.Client
{
    public class SecurityTokenRequestorAzureOAuth2 : ISecurityTokenRequestor
    {        
        private string appId = null;

        public SecurityTokenRequestorAzureOAuth2()
            : this(Constants.AZ_APPID_RLIL)
        {

        }

        public SecurityTokenRequestorAzureOAuth2(string appId)
        {
            this.appId = appId;
        }


        public SecurityToken RequestToken(string issuer, string appliesTo, RecordsManagerCredentials credentials)
        {
            var authContext = new AuthenticationContext(Constants.WAAD_AUTHORITY);

            AuthenticationResult result = null;

            // first, try to get a token silently
            try
            {
                result = authContext.AcquireTokenSilentAsync(appId, Constants.WAAD_CLIENTID).Result;
            }
            catch (AggregateException exc)
            {
                AdalException ex = exc.InnerException as AdalException;

                // There is no token in the cache; prompt the user to sign-in.
                if (ex != null && ex.ErrorCode != "failed_to_acquire_token_silently")
                {
                    throw;
                }
            }

            if (result == null)
            {
                result = this.RunSync(async () =>
                {
                    return await authContext.AcquireTokenAsync(appId, Constants.WAAD_CLIENTID, new UserPasswordCredential(credentials.Username, credentials.Password));
                });
            }

            return new SecurityToken(result.AccessToken, result.AccessTokenType, result.ExpiresOn.DateTime);
        }

        private T RunSync<T>(Func<Task<T>> func)
        {
            return Task.Run(() =>
            {
                return func();
            }).Result;
        }
    }
}