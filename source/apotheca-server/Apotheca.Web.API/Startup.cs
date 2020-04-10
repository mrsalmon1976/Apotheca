using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apotheca.Web.API.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Apotheca.BLL.Repositories;
using Apotheca.BLL.Security;
using Apotheca.BLL.Validators;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson;
using Apotheca.Web.API.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using NLog;
using Microsoft.AspNetCore.Diagnostics;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Apotheca.Auth;

namespace Apotheca.Web.API
{
    public class Startup
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // todo: make origins configurable
            services.AddCors(o => o.AddPolicy("ApothecaCorsPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // configure strongly typed settings objects
            var congnitoSettingsSection = Configuration.GetSection("AWS");
            var cognitoSettings = congnitoSettingsSection.Get<CognitoSettings>();

            // configure application settings
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            appSettings.CognitoSettings = cognitoSettings;
            services.AddSingleton<IAppSettings>(appSettings);


            RegionEndpoint regionEndPoint = RegionEndpoint.GetBySystemName(appSettings.CognitoSettings.Region);
            var cognitoIdentityProvider = new AmazonCognitoIdentityProviderClient(regionEndPoint);
            services.AddSingleton<IAmazonCognitoIdentityProvider>(cognitoIdentityProvider);

            CognitoUserPool cognitoUserPool = new CognitoUserPool(appSettings.CognitoSettings.UserPoolId, appSettings.CognitoSettings.AppClientId, cognitoIdentityProvider, appSettings.CognitoSettings.AppClientSecret);
            services.AddSingleton<CognitoUserPool>(cognitoUserPool);

            // set up JWT auth
            services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.Audience = appSettings.CognitoSettings.AppClientId;
                    options.Authority = $"https://cognito-idp.{appSettings.CognitoSettings.Region}.amazonaws.com/{appSettings.CognitoSettings.UserPoolId}";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true
                    };
                });

            services.AddCognitoIdentity();

            // providers
            services.AddScoped<IPasswordProvider, PasswordProvider>();
            services.AddScoped<IAmazonCognitoProvider>((s) => new AmazonCognitoProvider(appSettings.CognitoSettings.Region, appSettings.CognitoSettings.UserPoolId, appSettings.CognitoSettings.AppClientId, appSettings.CognitoSettings.AppClientSecret));

            // database
            IMongoClient mongoClient = new MongoClient("mongodb://apotheca:apotheca123@localhost/apotheca");
            services.AddSingleton<IMongoClient>(mongoClient);
            services.AddScoped<IMongoDatabase>((sp) => mongoClient.GetDatabase("apotheca"));
            // serialize enums as strings by default
            var pack = new ConventionPack 
            {
                new EnumRepresentationConvention(BsonType.String)
            };
            ConventionRegistry.Register("EnumStringConvention", pack, t => true);

            // repositories
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // validators
            services.AddScoped<IEmailValidator, EmailValidator>();
            services.AddScoped<IUserValidator, UserValidator>();

            // domain services
            // services.AddScoped<IUserService, UserService>();

            // application services 
            services.AddScoped<IAccountViewModelService, AccountViewModelService>();

            services.ConfigureApplicationCookie(options => {
                //options.AccessDeniedPath = "/Account/Login";
                //options.LoginPath = "/Account/Denied";
                //options.Cookie.HttpOnly = true;
                //options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.Events.OnRedirectToLogin = context => {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            AppMap.Configure();

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors("ApothecaCorsPolicy");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var feature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = feature.Error;
                _logger.Error(exception, exception?.Message);
                var result = JsonConvert.SerializeObject(new { message = exception.Message, stackTrace = exception.StackTrace });
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }));
            app.UseMvc();

        }

    }
}
