using System;
using System.Collections.Generic;
using System.Linq;

namespace GridViewEditDemo.Classes
{
    /// <summary>
    /// This code is just to make the demo work with some test data. You need to implement your own to read the data from another source like a database.
    /// </summary>
    public class Books
    {
        /// <summary>
        /// Generates a list with random books
        /// </summary>
        /// <returns>List with the Book class</returns>
        public static List<Book> GetBooks()
        {
            //some random Lorem Ipsum words
            string[] values = "Lorem Ipsum Dolor Sit Amet Consectetur Adipiscing Elit Sed Eiusmod Tempor Incididunt Labore Dolore Magna Aliqua Enim Minim Veniam Quis Nostrud Exercitation Ullamco Laboris Nisi Aliquip Commodo Consequat".Split(' ');

            var books = new List<Book>();
            var rnd = new Random();
            var categories = GetBookCategories();

            //generate some books with random names, categories and dates
            for (int i = 1; i <= 10; i++)
            {
                books.Add(new Book()
                {
                    ID = i,
                    Title = $"{values[rnd.Next(0, values.Length)]} {values[rnd.Next(0, values.Length)]}",
                    Category = categories[rnd.Next(0, categories.Count())],
                    Date = DateTime.Now.AddDays(rnd.Next(-1825, 1825))
                });
            }

            return books;
        }


        /// <summary>
        /// Generates the list with the book categories
        /// </summary>
        /// <returns>List with the BookCategory class</returns>
        public static List<BookCategory> GetBookCategories()
        {
            string[] categories = "Action,Classic,Crime,Drama,Fantasy,Horror,Mystery,Philosophy,Romance,Science Fiction".Split(',');

            //split the categories string and select into to the class list BookCategory
            return categories.Select((name, index) => new BookCategory()
            {
                ID = index + 1,
                Name = name
            }).ToList();
        }


        public class Book
        {
            public int ID { get; set; }
            public string Title { get; set; }
            public BookCategory Category { get; set; }
            public DateTime Date { get; set; }
        }


        public class BookCategory
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }
    }
}