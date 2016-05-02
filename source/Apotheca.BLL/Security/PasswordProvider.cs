﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Security
{
    public interface IPasswordProvider
    {
        bool CheckPassword(string password, string hash);

        string GenerateSalt();

        string HashPassword(string password, string salt);
    }

    class PasswordProvider : IPasswordProvider
    {
        public bool CheckPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        public string GenerateSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt();
        }

        public string HashPassword(string password, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }
    }
}
