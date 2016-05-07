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
using Apotheca.BLL.Data;
using Apotheca.BLL.Commands.User;
using Apotheca.BLL.Validators;
using System.Collections.Generic;
using Apotheca.Navigation;
using SystemWrapper.IO;

namespace Apotheca
{

    public interface INancyContextWrapper
    {
        NancyContext Context { get; set; }
    }

    public class NancyContextWrapper : INancyContextWrapper
    {
        private NancyContext _context;
        public NancyContext Context
        {
            get { return _context; }
            set { _context = value; } //do sometSetupControllerou want to prevent repeated sets
        }
    }

    public class ApothecaBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            // register settings first as we will use that below
            IAppSettings settings = new AppSettings();
            container.Register<IAppSettings>(settings);

            // make sure we migrate database changes if there are any
            container.Register<IDbScriptResourceProvider, DbScriptResourceProvider>();

            // at this point, run in any database changes if there are any
            using (IDbContext dbContext = new DbContext(settings.ConnectionString, settings.DbSchema))
            {
                IDbScriptResourceProvider resourceProvider = container.Resolve<IDbScriptResourceProvider>();
                new DbMigrator().Migrate(dbContext, resourceProvider.GetDbMigrationScripts());
            }
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
            
            IAppSettings settings = container.Resolve<IAppSettings>();

            // IO Wrapper
            container.Register<IDirectoryWrap, DirectoryWrap>();
            container.Register<IPathWrap, PathWrap>();
            container.Register<IPathHelper, PathHelper>();

            // Apotheca classes and controllers
            container.Register<IUserMapper, UserMapper>();
            container.Register<IDashboardController, DashboardController>();
            container.Register<IDocumentController, DocumentController>();
            container.Register<ILoginController, LoginController>();
            container.Register<ISetupController, SetupController>();

            // BLL commands
            container.Register<ICreateUserCommand, CreateUserCommand>();

            // BLL repositories
            container.Register<IDocumentRepository, DocumentRepository>();
            container.Register<IUserRepository, UserRepository>();

            // other BLL classes
            container.Register<IUserValidator, UserValidator>();

            // register a DB context
            container.Register<IDbContext>(new DbContext(settings.ConnectionString, settings.DbSchema));
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            // set shared ViewBag details here
            context.ViewBag.AppVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            context.ViewBag.Scripts = new List<string>();
        }

    }
}
