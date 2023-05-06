using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksDb
{
    class Book
    {
        private int id = 0;
        private string name;
        private string author;
        private int year;
        private string genres;
        private string publisher;

        public int ID { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Author { get => author; set => author = value; }
        public int Year { get => year; set => year = value; }
        public string Genres { get => genres; set => genres = value; }
        public string Publisher { get => publisher; set => publisher = value; }
    }

    class Painting
    {
        private int id = 0;
        private string name;
        private string author;
        private int year;
        private int price;

        public int ID { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Author { get => author; set => author = value; }
        public int Year { get => year; set => year = value; }
        public int Price { get => price; set => price = value; }
    }
    class Author
    {
        private int id = 0;
        private string name;
        private int age;
        private DateTime birthday;
        private string country;
        private string language;

        public int ID { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int Age { get => age; set => age = value; }
        public DateTime Birthday { get => birthday; set => birthday = value; }
        public string Country { get => country; set => country = value; }
        public string Language { get => language; set => language = value; }
    }
}
