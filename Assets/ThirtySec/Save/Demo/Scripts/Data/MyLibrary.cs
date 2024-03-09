using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ThirtySec
{
    [System.Serializable]
    public struct MyBook
    {
        public string author;
        public string releaseDate;
        public string content;
        public MyBook(string author, string content, string releaseDate)
        {
            this.author = author;
            this.content = content;
            this.releaseDate = releaseDate;
        }
    }

    public class Library : ThirtySec.Serializable<Library>
    {
        public List<MyBook> myBooks = new List<MyBook>();
    }

   public class MyLibrary : MonoBehaviour
    {
        public MyBook bookToAdd;
        private void Start()
        {
            AddBook();
        }

        void AddBook()
        {
            //create a book
            //MyBook thirtySecBook = new MyBook("ThirtySec", "Lorem ipsum dolor sit amet", 2020);

            //add it to the library
            Library.instance.myBooks.Add(bookToAdd);

            //get books
            List<MyBook> books = Library.instance.myBooks;

            for (int i = 0; i < books.Count; i++)
            {
                Debug.LogFormat("Book [{0}]: Author {1}, Release Date: {2}, Content: {3}", i, books[i].author,books[i].releaseDate, books[i].content);
            }

        }
    }

}