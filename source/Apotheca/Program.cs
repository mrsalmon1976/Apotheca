using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Apotheca
{
    class Program
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            _logger.Info("Apotheca starting up");

            try
            {
                HostFactory.Run(
                    configuration =>
                    {
                        configuration.Service<ApothecaService>(
                            service =>
                            {
                                service.ConstructUsing(x => new ApothecaService());
                                service.WhenStarted(x => x.Start());
                                service.WhenStopped(x => x.Stop());
                            });

                        string serviceUserName = ConfigurationManager.AppSettings["ServiceUserName"];
                        string servicePassword = ConfigurationManager.AppSettings["ServicePassword"];

                        // run as a user if one is specified
                        if (serviceUserName.Length > 0 && servicePassword.Length > 0)
                        {
                            configuration.StartAutomatically();
                            configuration.RunAs(serviceUserName, servicePassword);
                        }
                        else
                        {
                            configuration.RunAsLocalSystem();
                        }

                        configuration.SetServiceName("Apotheca");
                        configuration.SetDisplayName("Apotheca document service");
                        configuration.SetDescription("The Apotheca service.");
                    });
                _logger.Info("Apotheca shutting down");
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Apotheca crashed!");
            }


        }
    }
}
