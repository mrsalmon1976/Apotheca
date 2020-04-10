using System;
using System.Collections.Generic;
using System.Text;

namespace Apotheca.Auth.Models
{
    public class LoginResult
    {
        public string AccessToken { get; set; }

        public string IdToken { get; set; }

        /// <summary>
        /// The expiration period of the authentication result in seconds.
        /// </summary>
        public int ExpiresIn { get; set; }
    }
}
