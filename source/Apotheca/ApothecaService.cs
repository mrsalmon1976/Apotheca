using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Hosting.Self;
using System.Net.Sockets;
using NLog; 

namespace Apotheca
{
    public class ApothecaService
    {
        private NancyHost _host;
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public void Start()
        {
            _logger.Info("Apotheca Windows Service starting");
            var hostConfiguration = new HostConfiguration
            {
                UrlReservations = new UrlReservations() { CreateAutomatically = true }
                
            };

            string url = "http://localhost:8910";
            _host = new NancyHost(hostConfiguration, new Uri(url));
            _host.Start();
            // code that runs when the Windows Service starts up
            //_webServer = WebApp.Start<Startup>("http://localhost:8084");
        }

        public void Stop()
        {
            // code that runs when the Windows Service stops
            //_webServer.Dispose();
            _logger.Info("Apotheca Windows Service shutting down");
            _host.Stop();
            _host.Dispose();
        }
    }
}
