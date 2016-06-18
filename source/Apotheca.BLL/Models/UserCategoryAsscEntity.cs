using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Models
{
    public class UserCategoryAsscEntity
    {
        public UserCategoryAsscEntity()
        {
        }

        public UserCategoryAsscEntity(Guid userId, Guid categoryId)
        {
            this.UserId = userId;
            this.CategoryId = categoryId;
        }
        

        public int Id { get; internal set; }

        public Guid UserId { get; set; }

        public Guid CategoryId { get; set; }

    }
}
