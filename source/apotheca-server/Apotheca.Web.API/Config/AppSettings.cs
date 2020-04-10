using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apotheca.Web.API.Config
{
    public interface IAppSettings
    {
        CognitoSettings CognitoSettings { get; set; }
    }
    public class AppSettings : IAppSettings
    {
        public CognitoSettings CognitoSettings { get; set; }
    }
}
