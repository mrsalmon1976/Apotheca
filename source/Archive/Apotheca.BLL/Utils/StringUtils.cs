using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Utils
{
    public class StringUtils
    {
        public static Guid[] ConvertToGuidArray(string guids, char delimiter)
        {
            return guids
            .Split(delimiter)
            .Where(g => { Guid temp; return Guid.TryParse(g, out temp); })
            .Select(g => Guid.Parse(g))
            .ToArray();
        }
    }
}
