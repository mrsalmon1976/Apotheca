using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apotheca.Web.API.Config
{
    public class CognitoSettings
    {
        public string UserPoolId { get; set; }

        public string AppClientId { get; set; }

        public string AppClientSecret { get; set; }

        public string Region { get; set; }

    }
}
