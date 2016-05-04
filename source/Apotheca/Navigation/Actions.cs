﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Navigation
{
    public class Actions
    {
        public const string Dashboard = "/dashboard";

        public class Document
        {
            public const string Default = "/document";

            public const string Add = "/document/add";
        }

        public class Login
        {
            public const string Default = "/login";
        }

        public class Setup
        {
            public const string Default = "/setup";
        }
    }
}