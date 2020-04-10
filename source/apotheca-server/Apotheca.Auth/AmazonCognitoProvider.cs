using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Apotheca.Auth.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Auth
{

    public interface IAmazonCognitoProvider
    {

        string Region { get; set; }

        string ClientId { get; set; }

        string ClientSecretId { get; set; }

        string UserPoolId { get; set; }

        Task<LoginResult> LoginAsync(string email, string password);

        Task<RegisterResult> RegisterAsync(string email, string password, string firstName, string lastName);

        Task<UserResult> GetUser(string accessToken);

    }

    public class AmazonCognitoProvider : IAmazonCognitoProvider
    {
        private readonly RegionEndpoint _regionEndPoint = null;

        public AmazonCognitoProvider(string region, string userPoolId, string clientId, string clientSecretId)
        {
            this.Region = region;
            this.UserPoolId = userPoolId;
            this.ClientId = clientId;
            this.ClientSecretId = clientSecretId;

            _regionEndPoint = RegionEndpoint.GetBySystemName(this.Region);

        }

        public string Region { get; set; }

        public string ClientId { get; set; }

        public string ClientSecretId { get; set; }

        public string UserPoolId { get; set; }


        public async Task<RegisterResult> RegisterAsync(string email, string password, string firstName, string lastName)
        { 
            var cognito = new AmazonCognitoIdentityProviderClient(_regionEndPoint);

            var request = new SignUpRequest
            {
                ClientId = this.ClientId,
                SecretHash = AmazonCognitoHashCalculator.GetSecretHash(email, this.ClientId, this.ClientSecretId),
                Username = email,
                Password = password,
            };

            // refer: https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
            request.UserAttributes.Add(new AttributeType() { Name = CognitoUserAttribute.Email, Value = email } );
            request.UserAttributes.Add(new AttributeType() { Name = CognitoUserAttribute.FirstName, Value = firstName });
            request.UserAttributes.Add(new AttributeType() { Name = CognitoUserAttribute.LastName, Value = lastName });
            SignUpResponse response = await cognito.SignUpAsync(request);

            RegisterResult result = new RegisterResult()
            {
                UUID = response.UserSub
            };
            return result;
        }

        public async Task<LoginResult> LoginAsync(string email, string password)
        {
            var cognito = new AmazonCognitoIdentityProviderClient(_regionEndPoint);

            var request = new AdminInitiateAuthRequest
            {
                UserPoolId = this.UserPoolId,
                ClientId = this.ClientId,
                AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH
            };

            request.AuthParameters.Add("USERNAME", email);
            request.AuthParameters.Add("PASSWORD", password);
            request.AuthParameters.Add("EMAIL", email);
            request.AuthParameters.Add("SECRET_HASH", AmazonCognitoHashCalculator.GetSecretHash(email, this.ClientId, this.ClientSecretId));


            var authResponse = await cognito.AdminInitiateAuthAsync(request);

            LoginResult loginResult = new LoginResult
            {
                AccessToken = authResponse.AuthenticationResult.AccessToken,
                IdToken = authResponse.AuthenticationResult.IdToken,
                ExpiresIn = authResponse.AuthenticationResult.ExpiresIn
            };
            return loginResult;
        }

        public Task<UserResult> GetUser(string accessToken)
        {
            var cognito = new AmazonCognitoIdentityProviderClient(_regionEndPoint);
            var userRequest = new GetUserRequest
            {
                AccessToken = accessToken
            };

            var userResponse = cognito.GetUserAsync(userRequest).Result;

            UserResult user = new UserResult();
            user.Email = userResponse.Username;
            user.FirstName = userResponse.UserAttributes.Find((x) => x.Name == CognitoUserAttribute.FirstName)?.Value;
            user.LastName = userResponse.UserAttributes.Find((x) => x.Name == CognitoUserAttribute.LastName)?.Value;
            return Task<UserResult>.FromResult<UserResult>(user);

        }


    }
}
