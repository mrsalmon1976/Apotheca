using Apotheca.BLL.Database;
using Apotheca.Configuration;
using Apotheca.Security;
using Apotheca.BLL.Resources;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Bootstrapper;
using Nancy.Security;
using Nancy.TinyIoc;
using System;
using System.Data;
using System.Data.SqlClient;
using Apotheca.BLL.Repositories;
using Apotheca.Controllers;
using Apotheca.ViewModels;
using System.Reflection;

namespace Apotheca
{

    public class ApothecaBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            // register settings first as we will use that below
            IAppSettings settings = new AppSettings();
            container.Register<IAppSettings>(settings);

            // Apotheca class
            container.Register<IUserMapper, UserMapper>();
            container.Register<IDashboardController, DashboardController>().AsMultiInstance();
            container.Register<ILoginController, LoginController>().AsMultiInstance();
            container.Register<IUserController, UserController>().AsMultiInstance();

            // BLL classes
            container.Register<IDbScriptResourceProvider, DbScriptResourceProvider>().AsMultiInstance();
            container.Register<IDbMigrator, DbMigrator>();
            container.Register<IUserRepository>((c, p) => new UserRepository(settings.DbSchema, container.Resolve<IDbConnection>()));

            // database connection next!
            container.Register<IDbConnection>((c, p) =>
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = settings.ConnectionString;
                conn.Open();
                return conn;
            });

            // the first thing to do is run database changes if there are any
            IDbScriptResourceProvider resourceProvider = container.Resolve<IDbScriptResourceProvider>();
            container.Resolve<IDbMigrator>().Migrate(settings.DbSchema, resourceProvider.GetDbMigrationScripts());
        }

        protected override void RequestStartup(TinyIoCContainer requestContainer, IPipelines pipelines, NancyContext context)
        {
            // register all global data here
            context.ViewBag.AppVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);

        }
    }
}
