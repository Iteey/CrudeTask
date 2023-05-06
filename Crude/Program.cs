using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksDb
{
    class Program
    {
        public static string bkfile = "Books.xml";
        public static string ptfile = "Paintings.xml";
        public static string aufile = "Authors.xml";
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                bkfile = args[0];
            }
            else if (args.Length == 2)
            {
                bkfile = args[0];
                ptfile = args[1];
            }
            else if (args.Length == 3)
            {
                bkfile = args[0];
                ptfile = args[1];
                aufile = args[2];
            }

            try
            {
                ConsoleMenuApp app = new DatabaseApp();
                app.Setup();
                app.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Fatal error: {e.Message}\n{e.StackTrace}");
                Console.ReadKey();
            }
        }

    }
}
