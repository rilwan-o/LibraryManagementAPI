using LibraryManagement.Domain.Interfaces.BookManager;
using LibraryManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryManagement.Domain.Services
{
    public class BookManagementService : IBookManagementService
    {
        private readonly List<Book> books;
        private readonly List<Author> authors;
        private readonly List<BookStore> bookstores;
        private readonly List<Publisher> publishers;
        public BookManagementService()
        {
            authors = new List<Author>()
            {
                new Author{Id = 1, FirstName="John", LastName="Doe"},
                new Author{Id = 2, FirstName="Jane", LastName="Mark"},
            };

            publishers = new List<Publisher>() { 
                new Publisher{Id = 1, Name="ORiley"},
                new Publisher{Id=2, Name="Wiley"}
            };

            bookstores = new List<BookStore>()
            {
                new BookStore{Id = 1, BookId =1, Quantity=3},
                new BookStore{Id = 2, BookId =2, Quantity=2},
                new BookStore{Id = 3, BookId =3, Quantity=4},
            };

            books = new List<Book>()
            {
                new Book{ Id =1, Name="Advanced Mathematics", AuthorId =1, PublisherId=1, PublishDate= new DateTime(2015, 12, 25)},
                new Book{ Id =2, Name="English Language", AuthorId =2, PublisherId=2, PublishDate= new DateTime(2015, 12, 25)},
                new Book{ Id =3, Name="Story Time", AuthorId =1, PublisherId=1, PublishDate= new DateTime(2019, 12, 25)},
                new Book{ Id =4, Name="Wonder Woman", AuthorId =1, PublisherId=1, PublishDate= new DateTime(2019, 12, 25)}
            };
        }
        public int AddAuthor(string firstName, string lastName)
        {
            var authur = authors.FirstOrDefault(a => a.FirstName.ToLower() == firstName.ToLower() && a.LastName.ToLower()==lastName.ToLower());
            if (authur != null) throw new Exception("Author already exists.");
            authur = new Author { FirstName = firstName, LastName = lastName };
            authur.Id = authors.Count + 1;
            authors.Add(authur);
            return authur.Id;
        }

        public List<Author> GetAuthors()
        {
            return authors;
        }

        public Author GetAuthor(int authorId)
        {
            return authors.FirstOrDefault(a => a.Id == authorId);
        }

        public int AddPublisher(string name)
        {
            var publisher = publishers.FirstOrDefault(p=>p.Name.ToLower() == name.ToLower());
            if (publisher != null) throw new Exception("Publisher already exists.");
            publisher = new Publisher { Name = name };
            publisher.Id = publishers.Count + 1;
            publishers.Add(publisher);
            return publisher.Id;
        }

        public List<Publisher> GetPublishers()
        {
            return publishers;
        }

        public Publisher GetPublisher(int publisherId)
        {
            return publishers.FirstOrDefault(p => p.Id == publisherId);
        }

        int IBookManagementService.CreateBook(Book book)
        {
            throw new NotImplementedException();
        }

        bool IBookManagementService.AddBookToStore(int bookId, int quantity)
        {
            throw new NotImplementedException();
        }

        bool IBookManagementService.RemoveBookFromStore(int bookId, int quantity)
        {
            throw new NotImplementedException();
        }

        Book IBookManagementService.GetBook(int id)
        {
            throw new NotImplementedException();
        }

        List<BookStore> IBookManagementService.GetBookStores()
        {
            throw new NotImplementedException();
        }
    }
}
