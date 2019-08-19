using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public interface ISecurityTokenRequestor
    {
        SecurityToken RequestToken(string issuer, string appliesTo, RecordsManagerCredentials credentials);
    }

    public class SecurityToken
    {
        public SecurityToken(string token, string tokenType)
            : this (token, tokenType, DateTime.Now.AddMinutes(1))
        {

        }

        public SecurityToken(string token, string tokenType, DateTime expiresOn)
            
        {
            this.Token = token;
            this.TokenType = tokenType;
            this.ExpiresOn = expiresOn;
        }

        public string Token { get; set; }

        public string TokenType { get; set; }

        public DateTime ExpiresOn { get; set; }
    }
}