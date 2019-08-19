using System;
using System.Linq;
using System.Net;

namespace RecordLion.RecordsManager.Client
{
    internal class WebClientWithCookies : WebClient
    {
        private int? timeout = null;

        public WebClientWithCookies()
        {
        }


        public WebClientWithCookies(int timeoutMilliseconds)
        {
            this.timeout = timeoutMilliseconds;
        }


        public WebClientWithCookies(CookieContainer cookieContainer)
        {
            this.CookieContainer = cookieContainer;
        }


        public WebClientWithCookies(int timeoutMilliseconds, CookieContainer cookieContainer)
        {
            this.timeout = timeoutMilliseconds;
            this.CookieContainer = cookieContainer;
        }


        public CookieContainer CookieContainer { get; set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);

            HttpWebRequest webRequest = request as HttpWebRequest;

            if (webRequest != null)
            {
                webRequest.CookieContainer = this.CookieContainer;

                if (this.timeout.HasValue)
                    webRequest.Timeout = this.timeout.Value;
            }

            return request;
        }
    }
}