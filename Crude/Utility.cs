using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BooksDb
{
    class Utility
    {
        public static string TruncateString(string str, int maxlength)
        {
            return (str.Length <= maxlength) ? str : (str.Substring(0, maxlength - 3) + "...");
        }
    }
}
