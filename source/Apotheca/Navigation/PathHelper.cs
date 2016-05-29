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
        string DownloadDirectory(string rootPath);

        string UploadDirectory(string rootPath);
    }

    public class PathHelper : IPathHelper
    {

        private IPathWrap _pathWrap;

        public PathHelper(IPathWrap pathWrap)
        {
            _pathWrap = pathWrap;
        }

        public string DownloadDirectory(string rootPath)
        {
            string path = _pathWrap.Combine(rootPath, "Content");
            return _pathWrap.Combine(path, "Downloads");

        }

        public string UploadDirectory(string rootPath)
        {
            string path = _pathWrap.Combine(rootPath, "Content");
            return _pathWrap.Combine(path, "Uploads");

        }
    }
}
