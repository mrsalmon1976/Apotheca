using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Web.Results
{
    public class TextResult
    {
        public TextResult(string text)
        {
            this.Text = text;
        }

        public string Text { get; set; }
    }
}
