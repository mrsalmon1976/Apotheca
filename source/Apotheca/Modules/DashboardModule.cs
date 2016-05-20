﻿using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
using Apotheca.Controllers;
using Apotheca.Navigation;
using Apotheca.ViewModels.Dashboard;
using Apotheca.Web.Results;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Modules
{
    public class DashboardModule : ApothecaSecureFormModule
    {
        public DashboardModule(IDashboardController dashboardController) : base()//, dashboardController)
        {
            Get[Actions.Dashboard, true] = async (x, ct) =>
            {
                this.RequiresAnyClaim(Roles.AllRoles);
                IControllerResult result = await dashboardController.HandleDashboardGetAsync();
                return this.HandleResult(result);
            };
        }
    }
}
