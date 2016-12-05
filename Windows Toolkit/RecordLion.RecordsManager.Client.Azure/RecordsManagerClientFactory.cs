using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RecordLion.RecordsManager.Client
{
    public static class RecordsManagerClientFactory
    {
        public static RecordsManagerClient Create(string url)
        {
            return new RecordsManagerClient(url);
        }

        public static RecordsManagerClient Create(string url, CookieContainer cookieContainer)
        {
            return new RecordsManagerClient(url, cookieContainer);
        }

        public static RecordsManagerClient Create(string url, RecordsManagerCredentials credentials)
        {
            if (url.TrimEnd('/').Equals(Constants.AZ_URL_RLIL, StringComparison.OrdinalIgnoreCase))
            {
                return new RecordsManagerClient(url, credentials, new SecurityTokenRequestorAzureOAuth2());
            }
            else if (url.TrimEnd('/').Equals(Constants.AZ_URL_RLIL_TEST, StringComparison.OrdinalIgnoreCase))
            {
                return new RecordsManagerClient(url, credentials, new SecurityTokenRequestorAzureOAuth2(Constants.AZ_URL_RLIL_TEST));
            }
            else
            {
                return new RecordsManagerClient(url, credentials);
            }
        }
    }
}
