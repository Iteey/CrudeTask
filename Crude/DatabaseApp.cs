using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace BooksDb
{
    public class DatabaseApp : ConsoleMenuApp
    {
        List<Book> books = new List<Book>();
        List<Painting> paintings = new List<Painting>();
        List<Author> authors = new List<Author>();
        string bkfile = Program.bkfile;
        string ptfile = Program.ptfile;
        string aufile = Program.aufile;

        protected override void AppSetup()
        {
            ConsoleMenu bk = new ConsoleMenu("Book");
            menus.Add(bk);

            bk.RegisterMenuItem("Read XML file", ReadXml);
            bk.RegisterMenuItem("Save XML file", SaveXmlOfType);
            bk.RegisterMenuItem("Change XML file", ChangeFile);
            bk.RegisterMenuItem("List books", List);
            bk.RegisterMenuItem("Add book", Add);
#if DEBUG
            bk.RegisterMenuItem("Add test books", AddTests);
#endif
            bk.RegisterMenuItem("Edit book", Change);
            bk.RegisterMenuItem("Delete book", Delete);
            bk.RegisterMenuItem("Search books", Search);
            bk.RegisterMenuItem("Delete all books", DeleteAll);
            AddExitToMenu(bk);

            RegisterSubmenu(main_menu, bk, "Book management");


            ConsoleMenu pt = new ConsoleMenu("Painting");
            menus.Add(pt);

            pt.RegisterMenuItem("Read XML file", ReadXml);
            pt.RegisterMenuItem("Save XML file", SaveXmlOfType);
            pt.RegisterMenuItem("Change XML file", ChangeFile);
            pt.RegisterMenuItem("List paintings", List);
            pt.RegisterMenuItem("Add painting", Add);
#if DEBUG
            pt.RegisterMenuItem("Add test paintings", AddTests);
#endif
            pt.RegisterMenuItem("Edit painting", Change);
            pt.RegisterMenuItem("Delete painting", Delete);
            pt.RegisterMenuItem("Search paintings", Search);
            pt.RegisterMenuItem("Delete all paintings", DeleteAll);

            AddExitToMenu(pt);

            RegisterSubmenu(main_menu, pt, "Painting management");


            ConsoleMenu au = new ConsoleMenu("Author");
            menus.Add(au);

            au.RegisterMenuItem("Read XML file", ReadXml);
            au.RegisterMenuItem("Save XML file", SaveXmlOfType);
            au.RegisterMenuItem("Change XML file", ChangeFile);
            au.RegisterMenuItem("List authors", List);
            au.RegisterMenuItem("Add author", Add);
#if DEBUG
            au.RegisterMenuItem("Add test authors", AddTests);
#endif
            au.RegisterMenuItem("Edit author", Change);
            au.RegisterMenuItem("Delete author", Delete);
            au.RegisterMenuItem("Search authors", Search);
            au.RegisterMenuItem("Delete all authors", DeleteAll);

            AddExitToMenu(au);

            RegisterSubmenu(main_menu, au, "Author management");
        }
        protected override void AppExit()
        {
            bool save = Confirm("Do you want to save changes to an XML document?");
            if (save)
            {
                Console.WriteLine("Saving and Exiting...");
                SaveXml();

            }
            else
            {
                Console.WriteLine("Exiting...");
            }
        }
        bool Confirm(string message)
        {
            Console.WriteLine(message + $"\t(Y/N)");
            string save = Console.ReadLine();
            if (save == "Y" || save == "y") return true;
            else return false;
        }
        void ChangeFile()
        {
            if (GetCurrentMenu() == "Book")
            {
                Console.WriteLine("File name (without .xml): ");
                string fname = Console.ReadLine() + ".xml";
                bkfile = fname;
            }
            else if (GetCurrentMenu() == "Painting")
            {
                Console.WriteLine("File name (without .xml): ");
                string fname = Console.ReadLine() + ".xml";
                ptfile = fname;
            }
            else if (GetCurrentMenu() == "Author")
            {
                Console.WriteLine("File name (without .xml): ");
                string fname = Console.ReadLine() + ".xml";
                aufile = fname;
            }
        }
        void Search()
        {
            if (GetCurrentMenu() == "Book")
            {
                if (books.Count == 0)
                {
                    Console.WriteLine("You have nothing to search for.");
                    return;
                }
                Console.Write("Enter a keyword to search by: ");
                string keyword = Console.ReadLine();
                foreach (Book bk in books)
                {
                    if (bk.ID.ToString().Contains(keyword) || bk.Name.Contains(keyword) || bk.Author.Contains(keyword) || bk.Year.ToString().Contains(keyword) ||
                        bk.Genres.Contains(keyword) || bk.Publisher.Contains(keyword))
                    {
                        Console.WriteLine($"ID: {bk.ID}\nName: {bk.Name}\nAuthor: {bk.Author}\nYear: {bk.Year}\n" +
                            $"Genres: {bk.Genres}\nPublisher: {bk.Publisher}\n");
                    }
                }
            }
            else if (GetCurrentMenu() == "Painting")
            {
                if (paintings.Count == 0)
                {
                    Console.WriteLine("You have nothing to search for.");
                    return;
                }
                Console.Write("Enter a keyword to search by: ");
                string keyword = Console.ReadLine();
                foreach (Painting pt in paintings)
                {
                    if (pt.ID.ToString().Contains(keyword) || pt.Name.Contains(keyword) || pt.Author.Contains(keyword) || pt.Year.ToString().Contains(keyword) ||
                        pt.Price.ToString().Contains(keyword))
                    {
                        Console.WriteLine($"ID: {pt.ID}\nName: {pt.Name}\nAuthor: {pt.Author}\nYear: {pt.Year}\n" +
                            $"Price: {pt.Price}\n");
                    }
                }
            }
            else if (GetCurrentMenu() == "Author")
            {
                if (authors.Count == 0)
                {
                    Console.WriteLine("You have nothing to search for.");
                    return;
                }
                Console.Write("Enter a keyword to search by: ");
                string keyword = Console.ReadLine();
                foreach (Author au in authors)
                {
                    if (au.ID.ToString().Contains(keyword) || au.Name.Contains(keyword) || au.Age.ToString().Contains(keyword) || au.Birthday.ToString().Contains(keyword) ||
                        au.Country.Contains(keyword) || au.Language.Contains(keyword))
                    {
                        Console.WriteLine($"ID: {au.ID}\nName: {au.Name}\nAge: {au.Age}\nBirthday: {au.Birthday}\n" +
                            $"Country: {au.Country}\nLanguage: {au.Language}\n");
                    }
                }
            }
        }
        void ReadXml()
        {
            if (GetCurrentMenu() == "Book")
            {
                try
                {
                    books.Clear();
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(bkfile);

                    XmlNodeList xmlswlist = xmlDoc.SelectNodes("/BookDb/BookList/Book");

                    foreach (XmlNode node in xmlswlist)
                    {
                        Book bk = new Book()
                        {
                            Name = node.SelectSingleNode("Name").InnerText,
                            Author = node.SelectSingleNode("Author").InnerText,
                            Year = int.Parse(node.SelectSingleNode("Year").InnerText),
                            Genres = node.SelectSingleNode("Genres").InnerText,
                            Publisher = node.SelectSingleNode("Publisher").InnerText
                        };

                        books.Add(bk);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception caught: {e.Message}, {e.StackTrace}");
                }
            }
            else if (GetCurrentMenu() == "Painting")
            {
                try
                {
                    paintings.Clear();
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ptfile);

                    XmlNodeList xmlswlist = xmlDoc.SelectNodes("/PaintingDb/PaintingList/Painting");

                    foreach (XmlNode node in xmlswlist)
                    {
                        Painting pt = new Painting()
                        {
                            Name = node.SelectSingleNode("Name").InnerText,
                            Author = node.SelectSingleNode("Author").InnerText,
                            Year = int.Parse(node.SelectSingleNode("Year").InnerText),
                            Price = int.Parse(node.SelectSingleNode("Genres").InnerText)
                        };

                        paintings.Add(pt);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception caught: {e.Message}, {e.StackTrace}");
                }
            }
            else if (GetCurrentMenu() == "Author")
            {
                try
                {
                    authors.Clear();
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(aufile);

                    XmlNodeList xmlswlist = xmlDoc.SelectNodes("/AuthorDb/AuthorList/Author");

                    foreach (XmlNode node in xmlswlist)
                    {
                        Author au = new Author()
                        {
                            Name = node.SelectSingleNode("Name").InnerText,
                            Age = int.Parse(node.SelectSingleNode("Age").InnerText),
                            Birthday = DateTime.Parse(node.SelectSingleNode("Birthday").InnerText),
                            Country = node.SelectSingleNode("Country").InnerText,
                            Language = node.SelectSingleNode("Language").InnerText
                        };

                        authors.Add(au);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception caught: {e.Message}, {e.StackTrace}");
                }
            }
        }
        void SaveXmlOfType()
        {
            if (GetCurrentMenu() == "Book")
            { 
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                XmlWriter writer = XmlWriter.Create(bkfile, settings);
                writer.WriteStartDocument();
                writer.WriteStartElement("BookDb");
                writer.WriteStartElement("BookList");

                foreach (Book bk in books)
                {
                    writer.WriteStartElement("Book");
                    writer.WriteAttributeString("id", bk.ID.ToString());

                    writer.WriteStartElement("Name");
                    writer.WriteString(bk.Name);
                    writer.WriteEndElement();

                    writer.WriteStartElement("Author");
                    writer.WriteString(bk.Author);
                    writer.WriteEndElement();

                    writer.WriteStartElement("Year");
                    writer.WriteString(bk.Year.ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("Genres");
                    writer.WriteString(bk.Genres);
                    writer.WriteEndElement();

                    writer.WriteStartElement("Publisher");
                    writer.WriteString(bk.Publisher);
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteEndDocument();
                writer.Close();
                Console.WriteLine("Saved XML Successfully");
            }
            else if (GetCurrentMenu() == "Painting")
            { 
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                XmlWriter writer = XmlWriter.Create(ptfile, settings);
                writer.WriteStartDocument();
                writer.WriteStartElement("PaintingDb");
                writer.WriteStartElement("PaintingList");

                foreach (Painting pt in paintings)
                {
                    writer.WriteStartElement("Painting");
                    writer.WriteAttributeString("id", pt.ID.ToString());

                    writer.WriteStartElement("Name");
                    writer.WriteString(pt.Name);
                    writer.WriteEndElement();

                    writer.WriteStartElement("Author");
                    writer.WriteString(pt.Author);
                    writer.WriteEndElement();

                    writer.WriteStartElement("Year");
                    writer.WriteString(pt.Year.ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("Price");
                    writer.WriteString(pt.Price.ToString());
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteEndDocument();
                writer.Close();
                Console.WriteLine("Saved XML Successfully");
            }
            else if (GetCurrentMenu() == "Author")
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                XmlWriter writer = XmlWriter.Create(aufile, settings);
                writer.WriteStartDocument();
                writer.WriteStartElement("AuthorDb");
                writer.WriteStartElement("AuthorList");

                foreach (Author au in authors)
                {
                    writer.WriteStartElement("Author");
                    writer.WriteAttributeString("id", au.ID.ToString());

                    writer.WriteStartElement("Name");
                    writer.WriteString(au.Name);
                    writer.WriteEndElement();

                    writer.WriteStartElement("Age");
                    writer.WriteString(au.Age.ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("Birthday");
                    writer.WriteString(au.Birthday.ToString("o"));
                    writer.WriteEndElement();

                    writer.WriteStartElement("Country");
                    writer.WriteString(au.Country);
                    writer.WriteEndElement();

                    writer.WriteStartElement("Language");
                    writer.WriteString(au.Language);
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteEndDocument();
                writer.Close();
                Console.WriteLine("Saved XML Successfully");
            }
        }
        void SaveXml()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(bkfile, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("BookDb");
            writer.WriteStartElement("BookList");

            foreach (Book bk in books)
            {
                writer.WriteStartElement("Book");
                writer.WriteAttributeString("id", bk.ID.ToString());

                writer.WriteStartElement("Name");
                writer.WriteString(bk.Name);
                writer.WriteEndElement();

                writer.WriteStartElement("Author");
                writer.WriteString(bk.Author);
                writer.WriteEndElement();

                writer.WriteStartElement("Year");
                writer.WriteString(bk.Year.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("Genres");
                writer.WriteString(bk.Genres); 
                writer.WriteEndElement();

                writer.WriteStartElement("Publisher");
                writer.WriteString(bk.Publisher);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteEndDocument();
            writer.Close();

            writer = XmlWriter.Create(ptfile, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("PaintingDb");
            writer.WriteStartElement("PaintingList");

            foreach (Painting pt in paintings)
            {
                writer.WriteStartElement("Painting");
                writer.WriteAttributeString("id", pt.ID.ToString());

                writer.WriteStartElement("Name");
                writer.WriteString(pt.Name);
                writer.WriteEndElement();

                writer.WriteStartElement("Author");
                writer.WriteString(pt.Author);
                writer.WriteEndElement();

                writer.WriteStartElement("Year");
                writer.WriteString(pt.Year.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("Price");
                writer.WriteString(pt.Price.ToString());
                writer.WriteEndElement();

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteEndDocument();
            writer.Close();


            writer = XmlWriter.Create(aufile, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("AuthorDb");
            writer.WriteStartElement("AuthorList");

            foreach (Author au in authors)
            {
                writer.WriteStartElement("Author");
                writer.WriteAttributeString("id", au.ID.ToString());

                writer.WriteStartElement("Name");
                writer.WriteString(au.Name);
                writer.WriteEndElement();

                writer.WriteStartElement("Age");
                writer.WriteString(au.Age.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("Birthday");
                writer.WriteString(au.Birthday.ToString("o"));
                writer.WriteEndElement();

                writer.WriteStartElement("Country");
                writer.WriteString(au.Country);
                writer.WriteEndElement();

                writer.WriteStartElement("Language");
                writer.WriteString(au.Language);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteEndDocument();
            writer.Close();
            Console.WriteLine("Saved XMLs Successfully");
    }
        void Change()
        {
            if (GetCurrentMenu() == "Book")
            { 
                if (books.Count == 0)
                {
                    Console.WriteLine("You don't have anything to change.");
                    return;
                }

                int index;

                do
                {
                    Console.WriteLine("Enter an index of item to change:");
                }
                while (!int.TryParse(Console.ReadLine(), out index) || books.Count <= index - 1 || index - 1 < 0);

                index -= 1;

                Book bk = books[index];

                Console.WriteLine($"{bk.Name}\t{bk.Author}\t{bk.Year}\t" + $"{bk.Genres}\t{bk.Publisher}");

                Console.WriteLine("Don't write anything if you want to keep the value");
                int bkId = bk.ID;
                Console.Write("Name:");
                string bkName = Console.ReadLine();
                bkName = bkName == "" ? bk.Name : bkName;

                Console.Write("Author:");
                string bkAuthor = Console.ReadLine();
                bkAuthor = bkAuthor == "" ? bk.Author : bkAuthor;

                int bkYear = 0;
                bool flag = true;
                while (flag) 
                {
                    Console.Write("Year:");
                    string strYear = Console.ReadLine();
                    if (strYear == "")
                    {
                        bkYear = bk.Year;
                        flag = false;
                    }
                    else if (!int.TryParse(strYear, out bkYear))
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                Console.Write("Genres:");
                string bkGenres = Console.ReadLine();
                bkGenres = bkGenres == "" ? bk.Genres : bkGenres;

                Console.Write("Publisher:");
                string bkPublisher = Console.ReadLine();
                bkPublisher = bkPublisher == "" ? bk.Publisher : bkPublisher;

                books.RemoveAt(index);
                books.Insert(index,
                    new Book()
                    {
                        ID = bkId,
                        Name = bkName,
                        Author = bkAuthor,
                        Year = bkYear,
                        Genres = bkGenres,
                        Publisher = bkPublisher
                    }
                    );
                }
            else if (GetCurrentMenu() == "Painting")
            {
                if (paintings.Count == 0)
                {
                    Console.WriteLine("You don't have anything to change.");
                    return;
                }

                int index;

                do
                {
                    Console.WriteLine("Enter an index of item to change:");
                }
                while (!int.TryParse(Console.ReadLine(), out index) || paintings.Count <= index - 1 || index - 1 < 0);

                index -= 1;

                Painting pt = paintings[index];

                Console.WriteLine($"{pt.Name}\t{pt.Author}\t{pt.Year}\t{pt.Price}");

                Console.WriteLine("Don't write anything if you want to keep the value");
                int ptId = pt.ID;
                Console.Write("Name:");
                string ptName = Console.ReadLine();
                ptName = ptName == "" ? pt.Name : ptName;

                Console.Write("Author:");
                string ptAuthor = Console.ReadLine();
                ptAuthor = ptAuthor == "" ? pt.Author : ptAuthor;

                int ptYear = 0;
                bool flag = true;
                while (flag)
                {
                    Console.Write("Year:");
                    string strYear = Console.ReadLine();
                    if (strYear == "")
                    {
                        ptYear = pt.Year;
                        flag = false;
                    }
                    else if (!int.TryParse(strYear, out ptYear))
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }

                int ptPrice = 0;
                bool flag2 = true;
                while (flag2)
                {
                    Console.Write("Price:");
                    string strPrice = Console.ReadLine();
                    if (strPrice == "")
                    {
                        ptPrice = pt.Price;
                        flag2 = false;
                    }
                    else if (!int.TryParse(strPrice, out ptPrice))
                    {
                        flag2 = true;
                    }
                    else
                    {
                        flag2 = false;
                    }
                }

                paintings.RemoveAt(index);
                paintings.Insert(index,
                    new Painting()
                    {
                        ID = ptId,
                        Name = ptName,
                        Author = ptAuthor,
                        Year = ptYear,
                        Price = ptPrice
                    }
                    );
            }
            else if (GetCurrentMenu() == "Author")
            {
                if (authors.Count == 0)
                {
                    Console.WriteLine("You don't have anything to change.");
                    return;
                }

                int index;

                do
                {
                    Console.WriteLine("Enter an index of item to change:");
                }
                while (!int.TryParse(Console.ReadLine(), out index) || authors.Count <= index - 1 || index - 1 < 0);

                index -= 1;

                Author au = authors[index];

                Console.WriteLine($"{au.Name}\t{au.Age}\t{au.Birthday}\t{au.Country}\t{au.Language}");

                Console.WriteLine("Don't write anything if you want to keep the value");
                int auId = au.ID;
                Console.Write("Name: ");
                string auName = Console.ReadLine();
                auName = auName == "" ? au.Name : auName;

                int auAge = 0;
                bool flag = true;
                while (flag)
                {
                    Console.Write("Age: ");
                    string strAge = Console.ReadLine();
                    if (strAge == "")
                    {
                        auAge = au.Age;
                        flag = false;
                    }
                    else if (!int.TryParse(strAge, out auAge))
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }

                DateTime auBirthday = new DateTime();
                flag = true;
                while (flag)
                {
                    Console.Write("Birthday: ");
                    string auBirthday_ = Console.ReadLine();
                    if (auBirthday_ == "")
                    {
                        auBirthday = au.Birthday;
                        flag = false;
                    }
                    else if (!DateTime.TryParse(auBirthday_, out auBirthday))
                    {
                        Console.WriteLine("Incorrect date. (Ex: 2021.13.10)");
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }

                Console.Write("Country: ");
                string auCountry = Console.ReadLine();
                auCountry = auCountry == "" ? au.Country : auCountry;

                Console.Write("Language: ");
                string auLanguage = Console.ReadLine();
                auLanguage = auLanguage == "" ? au.Language : auLanguage;

                authors.RemoveAt(index);
                authors.Insert(index,
                new Author()
                {
                    ID = auId,
                    Name = auName,
                    Age = auAge,
                    Birthday = auBirthday,
                    Country = auCountry,
                    Language = auLanguage
                }
                );
            }
            Console.WriteLine("Changed successfully.");
        }
        public void List()
        {
            if (GetCurrentMenu() == "Book")
            {
                Console.WriteLine("Listing all books...");

                StringBuilder sb = new StringBuilder(100);
                sb.Append('=', 100);

                string border = sb.ToString();

                string header = BookFormatTableEntry("Name", "Author", "Year", "Genres", "Publisher");

                Console.WriteLine(border);
                Console.WriteLine(header);
                Console.WriteLine(border);
                foreach (Book bk in books)
                {
                    Console.WriteLine(BookFormatTableEntry(bk.Name, bk.Author, bk.Year.ToString(), bk.Genres, bk.Publisher));
                    Console.WriteLine(border);
                }
            }
            else if (GetCurrentMenu() == "Painting")
            {
                Console.WriteLine("Listing all paintings...");

                StringBuilder sb = new StringBuilder(100);
                sb.Append('=', 80);

                string border = sb.ToString();

                string header = PaintingFormatTableEntry("Name", "Author", "Year", "Price");

                Console.WriteLine(border);
                Console.WriteLine(header);
                Console.WriteLine(border);
                foreach (Painting pt in paintings)
                {
                    Console.WriteLine(PaintingFormatTableEntry(pt.Name, pt.Author, pt.Year.ToString(), pt.Price.ToString()));
                    Console.WriteLine(border);
                }
            }
            else if (GetCurrentMenu() == "Author")
            {
                Console.WriteLine("Listing all authors...");

                StringBuilder sb = new StringBuilder(100);
                sb.Append('=', 80);

                string border = sb.ToString();

                string header = AuthorFormatTableEntry("Name", "Age", "Birthday", "Country", "Language");

                Console.WriteLine(border);
                Console.WriteLine(header);
                Console.WriteLine(border);
                foreach (Author au in authors)
                {
                    Console.WriteLine(AuthorFormatTableEntry(au.Name, au.Age.ToString(), au.Birthday.ToString(), au.Country, au.Language));
                    Console.WriteLine(border);
                }
            }
        }
        string PaintingFormatTableEntry(string name, string author, string year, string price)
        {

            StringBuilder sb = new StringBuilder(54);

            sb.Append("| "); // 20 + 20 + 4 + 10 = 54
            sb.Append($"{Utility.TruncateString(author, 20),-20}");
            sb.Append(" | ");
            sb.Append($"{Utility.TruncateString(name, 20),-20}");
            sb.Append(" | ");
            sb.Append($"{Utility.TruncateString(year, 4),-4}");
            sb.Append(" | ");
            sb.Append($"{Utility.TruncateString(price, 10),-10}");
            sb.Append(" |");

            return sb.ToString();
        }
        string BookFormatTableEntry(string name, string author, string year, string genres, string publisher)
        {

            StringBuilder sb = new StringBuilder(76);

            sb.Append("| "); // 20 + 20 + 4 + 20 + 12 = 76
            sb.Append($"{Utility.TruncateString(author, 20),-20}");
            sb.Append(" | ");
            sb.Append($"{Utility.TruncateString(name, 20),-20}");
            sb.Append(" | ");
            sb.Append($"{Utility.TruncateString(year, 4),-4}");
            sb.Append(" | ");
            sb.Append($"{Utility.TruncateString(genres, 20),-20}");
            sb.Append(" | ");
            sb.Append($"{Utility.TruncateString(publisher, 12),-12}");
            sb.Append(" |");

            return sb.ToString();
        }

        string AuthorFormatTableEntry(string name, string age, string birthday, string country, string language)
        {

            StringBuilder sb = new StringBuilder(63);

            sb.Append("| "); // 20 + 3 + 13 + 15 + 10 = 61
            sb.Append($"{Utility.TruncateString(name, 20),-20}");
            sb.Append(" | ");
            sb.Append($"{Utility.TruncateString(age, 3),-3}");
            sb.Append(" | ");
            sb.Append($"{Utility.TruncateString(birthday, 13),-13}");
            sb.Append(" | ");
            sb.Append($"{Utility.TruncateString(country, 15),-15}");
            sb.Append(" | ");
            sb.Append($"{Utility.TruncateString(language, 10),-10}");
            sb.Append(" |");

            return sb.ToString();
        }
        public void Add()
        {
            if (GetCurrentMenu() == "Book")
            { 
                int id = books.Count + 1;
                Console.Write("Enter book name: ");
                string name = Console.ReadLine();

                Console.Write("Enter author name: ");
                string author = Console.ReadLine();

                int year = 0;
                bool flag = true;
                while (flag)
                {
                    Console.Write("Enter release year: ");
                    string year_ = Console.ReadLine();
                    if (!int.TryParse(year_, out year))
                    {
                        Console.WriteLine("Incorrect year.");
                        flag = true;
                    }
                    else if (int.Parse(year_) < 0 || int.Parse(year_) > 2021)
                    {
                        Console.WriteLine("Incorrect year.");
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }

                Console.Write("Enter book genres: ");
                string genres = Console.ReadLine();

                Console.Write("Enter book publisher: ");
                string publisher = Console.ReadLine();

                books.Add(
                    new Book()
                    {
                        ID = id,
                        Name = name,
                        Author = author,
                        Year = year,
                        Genres = genres,
                        Publisher = publisher
                    }
                    );
            }
            else if (GetCurrentMenu() == "Painting")
            {
                int id = paintings.Count + 1;

                Console.Write("Enter painting name: ");
                string name = Console.ReadLine();

                Console.Write("Enter author name: ");
                string author = Console.ReadLine();

                int year = 0;
                bool flag = true;
                while (flag)
                {
                    Console.Write("Enter release year: ");
                    string year_ = Console.ReadLine();
                    if (!int.TryParse(year_, out year))
                    {
                        Console.WriteLine("Incorrect year.");
                        flag = true;
                    }
                    else if (int.Parse(year_) < 0 || int.Parse(year_) > 2021)
                    {
                        Console.WriteLine("Incorrect year.");
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }

                int price = 0;
                flag = true;
                while (flag)
                {
                    Console.Write("Enter painting price: ");
                    string price_ = Console.ReadLine();
                    if (!int.TryParse(price_, out price))
                    {
                        Console.WriteLine("Incorrect price.");
                        flag = true;
                    }
                    else if (int.Parse(price_) < 0 || int.Parse(price_) > Int32.MaxValue)
                    {
                        Console.WriteLine("Incorrect price.");
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }

                paintings.Add(
                    new Painting()
                    {
                        ID = id,
                        Name = name,
                        Author = author,
                        Year = year,
                        Price = price
                    }
                    );
            }
            else if (GetCurrentMenu() == "Author")
            {
                int id = authors.Count + 1;
                Console.Write("Enter author name: ");
                string name = Console.ReadLine();

                int age = 0;
                bool flag = true;
                while (flag)
                {
                    Console.Write("Enter author age: ");
                    string age_ = Console.ReadLine();
                    if (!int.TryParse(age_, out age))
                    {
                        Console.WriteLine("Incorrect age.");
                        flag = true;
                    }
                    else if (int.Parse(age_) < 0 || int.Parse(age_) > 150)
                    {
                        Console.WriteLine("Incorrect age.");
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }

                DateTime birthday = new DateTime();
                flag = true;
                while (flag)
                {
                    Console.Write("Enter birthday date: ");
                    string birthday_ = Console.ReadLine();
                    if (!DateTime.TryParse(birthday_, out birthday))
                    {
                        Console.WriteLine("Incorrect date. (Ex: 2021.13.10)");
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }

                Console.Write("Enter author country: ");
                string country = Console.ReadLine();

                Console.Write("Enter author language: ");
                string language = Console.ReadLine();

                authors.Add(
                    new Author()
                    {
                        ID = id,
                        Name = name,
                        Age = age,
                        Birthday = birthday,
                        Country = country,
                        Language = language
                    }
                    );
            }
        }
        void Delete()
        {
            if (GetCurrentMenu() == "Book")
            {
                if (books.Count == 0)
                {
                    Console.WriteLine("You don't have anything to delete.");
                    return;
                }
                while (true)
                {
                    Console.Write("Book to Delete:");
                    string bkDel = Console.ReadLine();
                    if (!int.TryParse(bkDel, out int result))
                    {
                        Console.WriteLine("Error: Please enter an integer");
                    }
                    if (books.Count <= result - 1 || result - 1 < 0)
                    {
                        Console.WriteLine("Error out of index");
                    }
                    else
                    {
                        bool del = Confirm("Are you sure?");
                        if (del)
                        {
                            books.RemoveAt(result - 1);
                            Console.WriteLine("Deleted successfully.");
                            break;
                        }
                        else break;
                    }
                }
            }
            else if (GetCurrentMenu() == "Painting")
            {
                if (paintings.Count == 0)
                {
                    Console.WriteLine("You don't have anything to delete.");
                    return;
                }
                while (true)
                {
                    Console.Write("Painting to Delete:");
                    string ptDel = Console.ReadLine();
                    if (!int.TryParse(ptDel, out int result))
                    {
                        Console.WriteLine("Error: Please enter an integer");
                    }
                    if (paintings.Count <= result - 1 || result - 1 < 0)
                    {
                        Console.WriteLine("Error out of index");
                    }
                    else
                    {
                        bool del = Confirm("Are you sure?");
                        if (del)
                        {
                            paintings.RemoveAt(result - 1);
                            Console.WriteLine("Deleted successfully.");
                            break;
                        }
                        else break;
                    }
                }
            }
            else if (GetCurrentMenu() == "Author")
            {
                if (authors.Count == 0)
                {
                    Console.WriteLine("You don't have anything to delete.");
                    return;
                }
                while (true)
                {
                    Console.Write("Author to Delete:");
                    string auDel = Console.ReadLine();
                    if (!int.TryParse(auDel, out int result))
                    {
                        Console.WriteLine("Error: Please enter an integer");
                    }
                    if (authors.Count <= result - 1 || result - 1 < 0)
                    {
                        Console.WriteLine("Error out of index");
                    }
                    else
                    {
                        bool del = Confirm("Are you sure?");
                        if (del)
                        {
                            authors.RemoveAt(result - 1);
                            Console.WriteLine("Deleted successfully.");
                            break;
                        }
                        else break;
                    }
                }
            }
        }
        void DeleteAll()
        {
            if (GetCurrentMenu() == "Book")
            {
                if (books.Count == 0)
                {
                    Console.WriteLine("You don't have anything to delete.");
                    return;
                }
                bool del = Confirm("Are you sure?");
                if (del)
                {
                    books.Clear();
                    Console.WriteLine("Deleted all books successfully.");
                }
                else Console.WriteLine("Delete all cancelled.");
            }

            else if (GetCurrentMenu() == "Painting")
            {
                if (paintings.Count == 0)
                {
                    Console.WriteLine("You don't have anything to delete.");
                    return;
                }
                bool del = Confirm("Are you sure?");
                if (del)
                {
                    paintings.Clear();
                    Console.WriteLine("Deleted all paintings successfully.");
                }
                else Console.WriteLine("Delete all cancelled.");
            }

            else if (GetCurrentMenu() == "Author")
            {
                if (authors.Count == 0)
                {
                    Console.WriteLine("You don't have anything to delete.");
                    return;
                }
                bool del = Confirm("Are you sure?");
                if (del)
                {
                    authors.Clear();
                    Console.WriteLine("Deleted all authors successfully.");
                }
                else Console.WriteLine("Delete all cancelled.");
            }
        }
        void AddTests()
        {
            if (GetCurrentMenu() == "Book")
            { 
                books.Add(
                new Book()
                {
                    ID = books.Count + 1,
                    Name = "Top 10 code languages",
                    Author = "howdi ho",
                    Year = 2004,
                    Genres = "Business",
                    Publisher = "Someone..."
                }
                );

                books.Add(
                new Book()
                {
                    ID = books.Count + 1,
                    Name = "Ctulhu",
                    Author = "lovecraft",
                    Year = 1928 ,
                    Genres = "Horror",
                    Publisher = "Another someone..."
                }
                );
            }
            else if (GetCurrentMenu() == "Painting")
            {
                paintings.Add(
                new Painting()
                {
                    ID = paintings.Count + 1,
                    Name = "Mona Lisa",
                    Author = "Leonardo Da Vinci",
                    Year = 1697,
                    Price = 850000000
                }
                );

                paintings.Add(
                new Painting()
                {
                    ID = paintings.Count + 1,
                    Name = "The Starry Night",
                    Author = "Vincent van Gogh",
                    Year = 1889,
                    Price = 100000000
                }
                );
            }
            else if (GetCurrentMenu() == "Author")
            {
                authors.Add(
                new Author()
                {
                    ID = authors.Count + 1,
                    Name = "Markus",
                    Age = 19,
                    Birthday = new DateTime(2004, 02, 03),
                    Country = "Estonia",
                    Language = "Estonia"
                }
                );

                authors.Add(
                new Author()
                {
                    ID = authors.Count + 1,
                    Name = "Doctor James",
                    Age = 54,
                    Birthday = new DateTime(1900, 06, 06),
                    Country = "Russia",
                    Language = "Russian"
                }
                );
            }
        }
    }
}
