using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Authentication.Forms;
using Apotheca.Controllers;
using Apotheca.ViewModels.Login;
using Nancy.ModelBinding;

namespace Apotheca.Modules
{
    public class SetupModule : ApothecaModule
    {
        public SetupModule(ISetupController setupController)
        {
            Get[Actions.Setup.Default] = x =>
            {
                return base.HandleResult(setupController.DefaultGet());
            };

            Post[Actions.Setup.Default] = x =>
            {
                return base.HandleResult(setupController.DefaultPost(this));
            };

        }
    }
}
