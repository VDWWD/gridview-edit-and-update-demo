using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using Newtonsoft.Json;

namespace GridViewEditDemo.Classes
{
    /// <summary>
    /// This code is just to make the demo work with some test data. You need to implement your own to read the data from another source like a database.
    /// </summary>
    public class Data
    {
        /// <summary>
        /// The path and name of the json file
        /// </summary>
        private static string BookFile
        {
            get
            {
                return HttpContext.Current.Server.MapPath("MyBooks.json");
            }
        }


        /// <summary>
        /// Try ty get the books list from disk. If not found create a new list and store it
        /// </summary>
        /// <returns>List with the Book class</returns>
        public static List<Books.Book> GetMyBooks()
        {
            //check if the file MyBooks.json exists
            if (File.Exists(BookFile))
            {
                //load the file and deserialize to the book list
                return JsonConvert.DeserializeObject<List<Books.Book>>(File.ReadAllText(BookFile));
            }
            else
            {
                //no file found then create a new list with random books
                var books = Books.GetBooks();

                //save the newly generated file
                SaveMyBooks(books);

                return books;
            }
        }


        /// <summary>
        /// Saves the list with the books to disk as json
        /// </summary>
        /// <param name="books">List with the Book class</param>
        public static void SaveMyBooks(List<Books.Book> books)
        {
            //serialize the books list and write it to disk
            File.WriteAllText(BookFile, JsonConvert.SerializeObject(books));
        }
    }
}