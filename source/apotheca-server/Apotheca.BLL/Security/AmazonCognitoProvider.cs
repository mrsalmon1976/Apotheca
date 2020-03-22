using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Security
{

    public interface IAmazonCognitoProvider
    {

        string Region { get; set; }

        string ClientId { get; set; }

        string ClientSecretId { get; set; }

        Task<SignUpResponse> RegisterAsync(string email, string password);

    }

    public class AmazonCognitoProvider : IAmazonCognitoProvider
    {
        public AmazonCognitoProvider(string region, string clientId, string clientSecretId)
        {
            this.Region = region;
            this.ClientId = clientId;
            this.ClientSecretId = clientSecretId;
        }

        public string Region { get; set; }

        public string ClientId { get; set; }

        public string ClientSecretId { get; set; }

        public async Task<SignUpResponse> RegisterAsync(string email, string password)
        { 
            RegionEndpoint regionEndPoint = RegionEndpoint.GetBySystemName(this.Region);
            var cognito = new AmazonCognitoIdentityProviderClient(regionEndPoint);

            var request = new SignUpRequest
            {
                ClientId = this.ClientId,
                SecretHash = AmazonCognitoHashCalculator.GetSecretHash(email, this.ClientId, this.ClientSecretId),
                Username = email,
                Password = password,
            };

            var emailAttribute = new AttributeType
            {
                Name = "email",
                Value = email
            };
            request.UserAttributes.Add(emailAttribute);
            return await cognito.SignUpAsync(request);
        }


    }
}
