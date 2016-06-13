using Apotheca.BLL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.ViewModels.User
{
    public class UserListViewModel : BaseViewModel
    {
        public UserListViewModel()
            : base()
        {
            this.Users = new List<UserSearchResult>();
        }

        public List<UserSearchResult> Users { get; private set; }
        

    }
}
