﻿using Apotheca.BLL.Repositories;
using Apotheca.Controllers;
using Apotheca.Navigation;
using Apotheca.ViewModels.Dashboard;
using Apotheca.Web.Results;
using Nancy;
using Nancy.Authentication.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Modules
{
    public class DashboardModule : ApothecaSecureFormModule
    {
        public DashboardModule(IUserMapper userMapper, IDashboardController dashboardController) : base(userMapper)
        {
            Get[Actions.Dashboard, true] = async (x, ct) =>
            {
                IControllerResult result = await dashboardController.HandleDashboardGetAsync();
                return this.HandleResult(result);
            };
        }
    }
}
