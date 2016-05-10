using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.ViewModels
{
    public class BaseViewModel
    {
        public BaseViewModel()
        {
            this.ValidationErrors = new List<string>();
        }

        public List<string> ValidationErrors { get; private set; }

    }
}
