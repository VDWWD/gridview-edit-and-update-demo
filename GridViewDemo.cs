using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace GridViewEditDemo
{
    //this class is just to make the demo work, it contains nothing needed by the gridview functionality
    public class GridViewDemo
    {
        public static List<Book> SortBooks(List<Book> books, string sortorder, SortDirection sortdirection)
        {
            if (sortorder == "ID" && sortdirection == SortDirection.Descending)
            {
                return books.OrderByDescending(x => x.ID).ToList();
            }
            else if (sortorder == "Title" && sortdirection == SortDirection.Descending)
            {
                return books.OrderByDescending(x => x.Title).ThenByDescending(x => x.ID).ToList();
            }
            else if (sortorder == "Title")
            {
                return books.OrderBy(x => x.Title).ThenBy(x => x.ID).ToList();
            }
            else if (sortorder == "Category" && sortdirection == SortDirection.Descending)
            {
                return books.OrderByDescending(x => x.CategoryName).ThenByDescending(x => x.Title).ToList();
            }
            else if (sortorder == "Category")
            {
                return books.OrderBy(x => x.CategoryName).ThenBy(x => x.Title).ToList();
            }
            else if (sortorder == "Date" && sortdirection == SortDirection.Descending)
            {
                return books.OrderByDescending(x => x.Date).ThenByDescending(x => x.Title).ToList();
            }
            else if (sortorder == "Date")
            {
                return books.OrderBy(x => x.Date).ThenBy(x => x.Title).ToList();
            }

            return books.OrderBy(x => x.ID).ToList();
        }


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


        public static List<BookCategory> GetBookCategories()
        {
            //some categories
            return new List<BookCategory>()
            {
                new BookCategory()
                {
                    ID = 1,
                    Name = "Action"
                },
                new BookCategory()
                {
                    ID = 2,
                    Name = "Classic Lit"
                },
                new BookCategory()
                {
                    ID = 3,
                    Name = "Crime"
                },
                new BookCategory()
                {
                    ID = 4,
                    Name = "Drama"
                },
                new BookCategory()
                {
                    ID = 5,
                    Name = "Fantasy"
                },
                new BookCategory()
                {
                    ID = 6,
                    Name = "Horror"
                },
                new BookCategory()
                {
                    ID = 7,
                    Name = "Mystery"
                },
                new BookCategory()
                {
                    ID = 89,
                    Name = "Philosophy"
                },
                new BookCategory()
                {
                    ID = 9,
                    Name = "Romance"
                },
                new BookCategory()
                {
                    ID = 10,
                    Name = "Science Fiction"
                },
            };
        }


        public class BookContainer
        {
            public int count { get; set; }
            public Guid guid { get; set; }
            public List<Book> books { get; set; }
        }


        public class Book
        {
            public int ID { get; set; }
            public string Title { get; set; }
            public BookCategory Category { get; set; }
            public DateTime Date { get; set; }

            public string CategoryName
            {
                get
                {
                    return Category != null ? Category.Name : null;
                }
            }
        }


        public class BookCategory
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }
    }
}