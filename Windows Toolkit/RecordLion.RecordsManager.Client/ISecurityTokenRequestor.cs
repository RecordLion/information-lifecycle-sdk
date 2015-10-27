using System;
using System.IdentityModel.Tokens;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public interface ISecurityTokenRequestor
    {
        GenericXmlSecurityToken RequestToken(string issuer, string appliesTo, RecordsManagerCredentials credentials);
    }
}