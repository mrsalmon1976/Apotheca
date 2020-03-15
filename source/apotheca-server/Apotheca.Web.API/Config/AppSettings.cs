using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apotheca.Web.API.Config
{
    public interface IAppSettings
    {
        string Secret { get; set; }

        CognitoSettings CognitoSettings { get; set; }
    }
    public class AppSettings : IAppSettings
    {
        public string Secret { get; set; }
        public CognitoSettings CognitoSettings { get; set; }
    }
}
