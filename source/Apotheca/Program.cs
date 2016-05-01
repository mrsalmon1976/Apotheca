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
        static void Main(string[] args)
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
        }
    }
}
