using System;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace RecordLion.RecordsManager.Client
{
    public class SPConnectorClient : ISPConnectorClient
    {
        #region fields
        
        private string baseUrl;
        private NetworkCredential credentials;
        private CookieContainer cookies;
        
        #endregion
        
        #region Constructors
        
        protected SPConnectorClient(string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentNullException("baseUrl");
            }
            
            this.baseUrl = baseUrl.TrimEnd('/');
            
            if (!this.baseUrl.EndsWith("/_vti_bin/rmconnector.svc", StringComparison.OrdinalIgnoreCase))
            {
                this.baseUrl += "/_vti_bin/rmconnector.svc";
            }
        }

        
        public SPConnectorClient(string spWebAppUrl, NetworkCredential credentials) : this(spWebAppUrl)
        {
            if (credentials == null)
            {
                throw new ArgumentNullException("credentials");
            }
            
            this.credentials = credentials;
        }

        
        public SPConnectorClient(string spWebAppUrl, CookieContainer cookies) : this(spWebAppUrl)
        {
            if (cookies == null)
            {
                throw new ArgumentNullException("cookies");
            }
            
            this.cookies = cookies;
        }

        #endregion

        #region ISPConnectorClient Members

        public string GetRecordManagerServerUrl()
        {
            string url = this.Get<string>("/v1/config/serverurl");

            return url;
        }


        public string GetDefaultZoneUri(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            Uri uri = null;

            if (Uri.TryCreate(path, UriKind.Absolute, out uri))
            {
                //Convert absolute URI to relative URI, so the server API will return the correct default zone URL
                path = uri.LocalPath;
            }

            var msgBody = new { path = path };

            string url = this.Post<string>("/v1/util/getdefaultzoneuri", msgBody);

            return url;
        }

        #endregion
        
        #region Private Methods
        
        private HttpWebRequest GetRequest(string resourceUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resourceUrl);
            request.Accept = "application/json";
            request.ContentType = "application/json";
            
            if (this.cookies == null)
            {
                if (this.credentials == null)
                {
                    throw new InvalidOperationException("Please set credentials before executing a request.");
                }
                
                request.Credentials = this.credentials;
            }
            else
            {
                request.CookieContainer = this.cookies;
            }
            
            return request;
        }
        

        private T Get<T>(string resourceUrl)
        {
            HttpWebRequest request = this.GetRequest(this.baseUrl + resourceUrl);
            
            HttpWebResponse response = null;
            
            try
            {
                response = request.GetResponse() as HttpWebResponse;
                
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    T result = JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
                    
                    return result;
                }
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }
        

        private T Post<T>(string resourceUrl, object data)
        {
            HttpWebRequest request = this.GetRequest(this.baseUrl + resourceUrl);
            request.Method = "POST";
            
            string jsonData = JsonConvert.SerializeObject(data);
            
            using (Stream s = request.GetRequestStream())
                using (StreamWriter sw = new StreamWriter(s))
                {
                    sw.Write(jsonData);
                    
                    sw.Flush();
                }
                
            HttpWebResponse response = null;
            
            try
            {
                response = request.GetResponse() as HttpWebResponse;
                
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    T result = JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
                    
                    return result;
                }
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }
        
        #endregion
    }
}