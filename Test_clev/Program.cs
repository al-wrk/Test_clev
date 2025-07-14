using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_clev
{
    internal class Program
    {
        static void Main()
        {
            string s = "aabccde";
            var compressed = s.Compress();
            var decompressed = compressed.Decompress();

            Console.WriteLine(s);
            Console.WriteLine(compressed);
            Console.WriteLine(decompressed);
        }
    }
}
