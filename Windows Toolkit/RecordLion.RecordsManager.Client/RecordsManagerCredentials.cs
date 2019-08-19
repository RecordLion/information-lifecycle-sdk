using System;
using System.Linq;
using System.Net;

namespace RecordLion.RecordsManager.Client
{
    [Serializable]
    public class RecordsManagerCredentials : ICredentials
    {
        public RecordsManagerCredentials() : this(null, null, RecordsManagerCredentialType.Claims)
        {
        }


        public RecordsManagerCredentials(string username, string password) : this(username, password, RecordsManagerCredentialType.Forms)
        {
        }


        public RecordsManagerCredentials(string username, string password, RecordsManagerCredentialType credentialType)
        {
            this.Username = username;
            this.Password = password;
            this.CredentialType = credentialType;
        }


        public string Username { get; private set; }

        public string Password { get; private set; }

        public RecordsManagerCredentialType CredentialType { get; private set; }

        public NetworkCredential GetCredential()
        {
            return this.GetCredential(null, null);
        }


        public NetworkCredential GetCredential(Uri uri, string authType)
        {
            if (!string.IsNullOrEmpty(this.Username))
            {
                if (this.Username.Contains("\\"))
                {
                    var parts = this.Username.Split('\\');

                    if (parts.Length > 2)
                        throw new Exception("Could not parse Username");

                    return new NetworkCredential(parts[1], this.Password, parts[0]);
                }
                else
                    return new NetworkCredential(this.Username, this.Password);
            }

            return CredentialCache.DefaultNetworkCredentials;
        }
    }
}