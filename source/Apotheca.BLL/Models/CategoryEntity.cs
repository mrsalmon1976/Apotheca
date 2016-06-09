using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Models
{
    public class CategoryEntity
    {
        public Guid Id { get; internal set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedOn { get; set; }

    }
}
