using System;
using System.Collections.Generic;
using System.Text;

namespace Apotheca.Auth
{
    /// <summary>
    /// Refer: https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
    /// </summary>
    public static class CognitoUserAttribute
    {
        public const string Email = "email";

        public const string FirstName = "given_name";

        public const string LastName = "family_name";


    }
}
