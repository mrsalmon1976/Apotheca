using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Apotheca.TestHelpers
{
    public class TestRandomHelper
    {
        public static int GetInt(int min, int max)
        {
            return new Random().Next(min, max);
        }

        public static byte[] GetFileContents(int length)
        {
            byte[] contents = new byte[length];
            new Random().NextBytes(contents);
            return contents;
        }
    }
}
