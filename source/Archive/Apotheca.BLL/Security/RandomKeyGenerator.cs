using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Security
{
    public interface IRandomKeyGenerator
    {
        string GenerateKey();
    }

    public class RandomKeyGenerator : IRandomKeyGenerator
    {
        public string GenerateKey()
        {
            var key = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(key);
                return Convert.ToBase64String(key);
            }
        }
    }
}
