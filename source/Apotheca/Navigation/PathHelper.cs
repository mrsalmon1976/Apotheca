using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemWrapper.IO;

namespace Apotheca.Navigation
{
    public interface IPathHelper
    {
        string UploadDirectory(string rootPath);
    }

    public class PathHelper : IPathHelper
    {

        private IPathWrap _pathWrap;

        public PathHelper(IPathWrap pathWrap)
        {
            _pathWrap = pathWrap;
        }

        public string UploadDirectory(string rootPath)
        {
            string path = _pathWrap.Combine(rootPath, "Content");
            return _pathWrap.Combine(path, "Uploads");

        }
    }
}
