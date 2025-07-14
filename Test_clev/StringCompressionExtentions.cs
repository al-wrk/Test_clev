using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Test_clev
{
    public static class StringCompressionExtentions
    {
        public static string Compress(this string s)
        {
            StringBuilder sb = new StringBuilder(s);
            int i = 0;
            while (i < sb.Length)
            {
                int startPos = i;
                int grouplenght = 1;
                while (i < sb.Length - 1 && sb[i] == sb[i+1])
                {
                    grouplenght++;
                    i++;
                }
                if (grouplenght > 1)
                {
                    sb.Remove(startPos + 1, grouplenght - 1);
                    sb.Insert(startPos + 1, grouplenght);
                    i = startPos + grouplenght.ToString().Length;
                }
                i++;
            }
            return sb.ToString();
        }
        public static string Decompress(this string s)
        {
            StringBuilder sb = new StringBuilder(s);
            int i = 0;
            while(i < sb.Length)
            {
                int startPos = i; // первая цифра
                int groupLenght = 0;
                while(i < sb.Length && Char.IsDigit(sb[i]))
                {
                    groupLenght *= 10;
                    groupLenght += sb[i] - '0';
                    i++;
                }
                if(groupLenght != 0)
                {
                    sb.Remove(startPos, groupLenght.ToString().Length);
                    char symbol = sb[startPos - 1];
                    sb.Insert(
                        index: startPos,
                        value: Enumerable.Repeat(symbol, groupLenght - 1).ToArray()
                        );
                    i = startPos + groupLenght - 1;
                }
                i++;
            }
            return sb.ToString();
        }
    }
}
