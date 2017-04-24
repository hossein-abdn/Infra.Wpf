using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestControls
{
    public class Product
    {
        static Random _rnd = new Random();
        static string[] _names = "1-Macko|2-Surfair|3-Pocohey|4-Studeby".Split('|');
        static string[] _lines = "Computers|Washers|Stoves|Cars".Split('|');
        static string[] _colors = "Red|Green|Blue|White".Split('|');
        static List<Person> _persons = new List<Person>
        {
            new Person { ID = 1, FirstName = "علی", LastName = "جعفری" },
            new Person { ID = 2, FirstName = "محمد", LastName = "عبدالهی" },
            new Person { ID = 3, FirstName = "a", LastName = "b" }
        };

        public Product()
        {
            Name = _names[_rnd.Next() % _names.Length];
            Line = _lines[_rnd.Next() % _lines.Length];
            Color = _colors[_rnd.Next() % _colors.Length];
            Price = _rnd.Next() % 10000000;
            Cost = 3 + _rnd.NextDouble() * 3000;
            Discontinued = _rnd.NextDouble() < .2;
            Introduced = DateTime.Now.AddDays(_rnd.Next(-600, 0)).AddHours(_rnd.Next(-23, 0));
            int i = _rnd.Next() % _persons.Count();
            Person = _persons.First(x => x == _persons.ElementAt(i));
        }

        public string Name { get; set; }

        public string Color { get; set; }

        public string Line { get; set; }

        public int Price { get; set; }

        public double Cost { get; set; }

        public DateTime Introduced { get; set; }

        public bool Discontinued { get; set; }

        public Person Person { get; set; }
    }

    public class Person
    {
        public int ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
