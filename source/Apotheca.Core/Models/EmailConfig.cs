using System;
using System.Collections.Generic;
using System.Text;

namespace Apotheca.Core.Models
{
    public class EmailConfig
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FromAddress { get; set; }
        public string FromDisplayName { get; set; }
    }
}
