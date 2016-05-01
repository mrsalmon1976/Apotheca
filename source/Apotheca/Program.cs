using NLog;
using System;
using System.Collections.Generic;
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
            _logger.Info("Apotheca starting up.");

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

                        configuration.RunAsLocalSystem();

                        configuration.SetServiceName("Apotheca");
                        configuration.SetDisplayName("Apotheca");
                        configuration.SetDescription("The Apotheca Document Store service.");
                    });
                _logger.Info("Apotheca shutting down.");
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Apotheca crashed!");
            }


        }
    }
}
