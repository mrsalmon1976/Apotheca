using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apotheca.BLL.Models
{
    public class FileEntry
    {
        public FileEntry()
        {
            this.Id = Guid.NewGuid();
        }

        /// <summary>
        /// Unique reference for this file entry.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Unique reference for a single file - common across all versions of a single file entry.
        /// </summary>
        public Guid FileId { get; set; }

        public string Name { get; set; }

        public bool IsFolder { get; set; }

        public long Length { get; set; }

        public string ContentType { get; set; }

        public Guid? Parent { get; set; }

        public string Location { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public int Version { get; set; }
    }
}
