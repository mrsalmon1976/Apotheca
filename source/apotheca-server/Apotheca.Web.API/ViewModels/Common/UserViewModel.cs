using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apotheca.Web.API.ViewModels.Common
{
    public class UserViewModel
    {

        public UserViewModel()
        {
            this.Stores = new List<StoreViewModel>();
        }

        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }

        public List<StoreViewModel> Stores { get; set; }


    }
}
