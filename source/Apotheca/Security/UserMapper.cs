using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Security
{
    public class UserMapper : IUserMapper
    {
        private IUserRepository _userRepo;

        public UserMapper(IUserRepository userRepo)
        {
            this._userRepo = userRepo;
        }

        public virtual IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            UserIdentity ui = null;
            UserEntity user = _userRepo.GetUserById(identifier);
            if (user != null)
            {
                ui = new UserIdentity();
                ui.Id = user.Id;
                ui.Claims = new string[] { user.Role };
                ui.UserName = user.Email;
            }
            return ui;
        }
    }
}
