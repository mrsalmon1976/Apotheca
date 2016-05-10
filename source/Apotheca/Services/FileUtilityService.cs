using Apotheca.Navigation;
using Nancy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemWrapper.IO;

namespace Apotheca.Services
{
    public interface IFileUtilityService
    {
        byte[] LoadUploadedFile(string rootPath, string fileName);

        void SaveUploadedFile(string rootPath, HttpFile file);
    }

    public class FileUtilityService : IFileUtilityService
    {
        private IPathHelper _pathHelper;
        private IDirectoryWrap _directoryWrap;
        private IPathWrap _pathWrap;
        private IFileWrap _fileWrap;

        public FileUtilityService(IPathHelper pathHelper, IDirectoryWrap directoryWrap, IPathWrap pathWrap, IFileWrap fileWrap)
        {
            _pathHelper = pathHelper;
            _directoryWrap = directoryWrap;
            _pathWrap = pathWrap;
            _fileWrap = fileWrap;
        }

        public byte[] LoadUploadedFile(string rootPath, string fileName)
        {
            var uploadDirectory = _pathHelper.UploadDirectory(rootPath);
            var filePath = _pathWrap.Combine(uploadDirectory, fileName);
            return _fileWrap.ReadAllBytes(filePath);
        }

        public void SaveUploadedFile(string rootPath, HttpFile file)
        {
            var uploadDirectory = _pathHelper.UploadDirectory(rootPath);

            if (!_directoryWrap.Exists(uploadDirectory))
            {
                _directoryWrap.CreateDirectory(uploadDirectory);
            }

            var filename = _pathWrap.Combine(uploadDirectory, file.Name);
            using (FileStream fileStream = new FileStream(filename, FileMode.Create))
            {
                file.Value.CopyTo(fileStream);
            }

        }
    }
}
