using Apotheca.BLL.Commands.Category;
using Apotheca.BLL.Commands.Document;
using Apotheca.BLL.Commands.User;
using Apotheca.BLL.Data;
using Apotheca.BLL.Database;
using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
using Apotheca.BLL.Resources;
using Apotheca.BLL.Validators;
using Apotheca.Configuration;
using Apotheca.Controllers;
using Apotheca.Navigation;
using Apotheca.Security;
using Apotheca.Services;
using Apotheca.ViewModels.Document;
using AutoMapper;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
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
                cfg.CreateMap<DocumentViewModel, DocumentEntity>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DocumentId));
                cfg.CreateMap<DocumentEntity, DocumentViewModel>().ForMember(dest => dest.DocumentId, opt => opt.MapFrom(src => src.Id));
            });

            // at this point, run in any database changes if there are any
            using (IDbConnection conn = new SqlConnection(settings.ConnectionString))
            {
                IDbScriptResourceProvider resourceProvider = container.Resolve<IDbScriptResourceProvider>();
                Console.WriteLine("Running migrations...");
                new DbMigrator().Migrate(conn, settings.DbSchema, resourceProvider.GetDbMigrationScripts());
                Console.WriteLine("...Done.");
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

            // set up a new DB Connection per request
            IDbConnection conn = new SqlConnection();
            conn.ConnectionString = settings.ConnectionString;
            conn.Open();
            container.Register<IDbConnection>(conn);

            // BLL repositories
            container.Register<ICategoryRepository>(new CategoryRepository(conn, settings.DbSchema));
            container.Register<IDocumentRepository>(new DocumentRepository(conn, settings.DbSchema));
            container.Register<IDocumentCategoryAsscRepository>(new DocumentCategoryAsscRepository(conn, settings.DbSchema));
            container.Register<IDocumentVersionRepository>(new DocumentVersionRepository(conn, settings.DbSchema));
            container.Register<IUserRepository>(new UserRepository(conn, settings.DbSchema));
            container.Register<IUserCategoryAsscRepository>(new UserCategoryAsscRepository(conn, settings.DbSchema));

            // set up the unit of work which will be used for database access
            IUnitOfWork unitOfWork = new UnitOfWork(conn, settings.DbSchema
                , container.Resolve<ICategoryRepository>()
                , container.Resolve<IDocumentRepository>()
                , container.Resolve<IDocumentCategoryAsscRepository>()
                , container.Resolve<IDocumentVersionRepository>()
                , container.Resolve<IUserRepository>()
                , container.Resolve<IUserCategoryAsscRepository>()
                );
            container.Register<IUnitOfWork>(unitOfWork);

            // Apotheca classes and controllers
            container.Register<IUserMapper, UserMapper>();
            container.Register<ICategoryController, CategoryController>();
            container.Register<IDashboardController, DashboardController>();
            container.Register<IDocumentController, DocumentController>();
            container.Register<ILoginController, LoginController>();
            container.Register<ISetupController, SetupController>();
            container.Register<IUserController, UserController>();

            // BLL commands
            container.Register<ISaveCategoryCommand, SaveCategoryCommand>();
            container.Register<ISaveDocumentCommand, SaveDocumentCommand>();
            container.Register<ISaveUserCommand, SaveUserCommand>();

            // other BLL classes
            container.Register<ICategoryValidator, CategoryValidator>();
            container.Register<IDocumentValidator, DocumentValidator>();
            container.Register<IUserValidator, UserValidator>();

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

            // clean up anything that needs to be
            pipelines.AfterRequest.AddItemToEndOfPipeline((ctx) =>
                {
                    IUnitOfWork uow = container.Resolve<IUnitOfWork>();
                    uow.Dispose();
                });
        }

    }
}
