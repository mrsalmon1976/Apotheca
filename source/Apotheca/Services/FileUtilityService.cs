using Apotheca.Navigation;
using Nancy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemWrapper;
using SystemWrapper.IO;

namespace Apotheca.Services
{
    public interface IFileUtilityService
    {
        int CleanUploadedFiles(string rootPath);

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

        /// <summary>
        /// Cleans up previously uploaded files.
        /// </summary>
        public int CleanUploadedFiles(string rootPath)
        {
            string uploadPath = _pathHelper.UploadDirectory(rootPath);
            string[] files =_directoryWrap.GetFiles(uploadPath);
            int count = 0;
            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.LastAccessTimeUtc < DateTime.UtcNow.AddDays(-1))
                {
                    _fileWrap.Delete(file);
                    count++;
                }
            }
            return count;
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
