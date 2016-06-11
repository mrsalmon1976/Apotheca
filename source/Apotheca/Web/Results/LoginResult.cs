﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Web.Results
{
    public class LoginResult : IControllerResult
    {
        public LoginResult()
        {
        }

        public bool Success { get; set; }
        public Guid UserId { get; set; }
    }
}
