using System;
using System.IdentityModel.Tokens;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;

namespace RecordLion.RecordsManager.Client
{
    public class DefaultSecurityTokenRequestor : ISecurityTokenRequestor
    {
        public GenericXmlSecurityToken RequestToken(string issuer, string appliesTo, RecordsManagerCredentials credentials)
        {
            var binding = (issuer.ToLower().StartsWith("https")) ? this.GetHttpsBinding() : this.GetHttpBinding();

            var address = new EndpointAddress(issuer);

            var factory = new WSTrustChannelFactory(binding, address);
            factory.TrustVersion = TrustVersion.WSTrust13;
            factory.Credentials.Windows.ClientCredential = credentials.GetCredential();

            var channel = factory.CreateChannel();

            var rst = new RequestSecurityToken(RequestTypes.Issue)
            {
                AppliesTo = new EndpointAddress(appliesTo)
            };

            RequestSecurityTokenResponse rstr = null;

            return channel.Issue(rst, out rstr) as GenericXmlSecurityToken;
        }


        private Binding GetHttpBinding()
        {
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;

            return binding;
        }


        private Binding GetHttpsBinding()
        {
            var binding = new WS2007HttpBinding(SecurityMode.Transport);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;

            return binding;
        }
    }
}