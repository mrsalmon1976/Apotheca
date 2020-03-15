using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Web.Results
{
    public class FileResult : IControllerResult
    {
        // public string ApplicationRelativeFilePath { get; set; }
        
        public string ContentType { get; set; }

        public string FileName { get; set; }

        public byte[] FileContents { get; set; }
    }
}
