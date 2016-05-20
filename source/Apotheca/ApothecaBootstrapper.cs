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
using Apotheca.Web;
using AutoMapper;
using Apotheca.ViewModels.Document;
using Apotheca.BLL.Models;
using Apotheca.Services;
using Apotheca.BLL.Commands.Document;

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

            // IO Wrapper
            container.Register<IDirectoryWrap, DirectoryWrap>();
            container.Register<IPathWrap, PathWrap>();
            container.Register<IFileWrap, FileWrap>();
            container.Register<IPathHelper, PathHelper>();

            // make sure we migrate database changes if there are any
            container.Register<IDbScriptResourceProvider, DbScriptResourceProvider>();

            // apotheca services
            container.Register<IFileUtilityService, FileUtilityService>();


            // set up mappings
            Mapper.Initialize((cfg) => {
                cfg.CreateMap<DocumentViewModel, DocumentEntity>();
            });

            // at this point, run in any database changes if there are any
            using (IDbContext dbContext = new DbContext(settings.ConnectionString, settings.DbSchema))
            {
                IDbScriptResourceProvider resourceProvider = container.Resolve<IDbScriptResourceProvider>();
                Console.Write("Running migrations...");
                new DbMigrator().Migrate(dbContext, resourceProvider.GetDbMigrationScripts());
                Console.WriteLine("Done.");
            }

            // delete old files
            IRootPathProvider rootPathProvider = container.Resolve<IRootPathProvider>();
            IFileUtilityService fileUtilityService = container.Resolve<IFileUtilityService>();
            Console.Write("Cleaning user uploads...");
            int files = fileUtilityService.CleanUploadedFiles(rootPathProvider.GetRootPath());
            Console.WriteLine("{0} file(s) deleted.", files);
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
            
            IAppSettings settings = container.Resolve<IAppSettings>();

            // Apotheca classes and controllers
            container.Register<IUserMapper, UserMapper>();
            container.Register<ICategoryController, CategoryController>();
            container.Register<IDashboardController, DashboardController>();
            container.Register<IDocumentController, DocumentController>();
            container.Register<ILoginController, LoginController>();
            container.Register<ISetupController, SetupController>();

            // BLL commands
            container.Register<ICreateDocumentCommand, CreateDocumentCommand>();
            container.Register<ICreateUserCommand, CreateUserCommand>();

            // BLL repositories
            container.Register<IDocumentRepository, DocumentRepository>();
            container.Register<IUserRepository, UserRepository>();

            // other BLL classes
            container.Register<IDocumentValidator, DocumentValidator>();
            container.Register<IUserValidator, UserValidator>();

            // register a DB context
            container.Register<IDbContext>(new DbContext(settings.ConnectionString, settings.DbSchema));
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            var formsAuthConfiguration = new FormsAuthenticationConfiguration()
            {
                RedirectUrl = "~/login",
                UserMapper = container.Resolve<IUserMapper>(),
            };
            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);

            // set shared ViewBag details here
            context.ViewBag.AppVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            context.ViewBag.Scripts = new List<string>();

        }

    }
}
