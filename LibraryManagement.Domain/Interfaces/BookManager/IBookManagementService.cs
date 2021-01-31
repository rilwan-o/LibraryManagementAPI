using LibraryManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Domain.Interfaces.BookManager
{
    public interface IBookManagementService
    {
        int AddAuthor(string firstName, string lastName);
        int AddPublisher(string name);

        int CreateBook(Models.Book book);
        bool AddBookToStore(int bookId, int quantity);
        bool RemoveBookFromStore(int bookId, int quantity);
        Book GetBook(int id);
        List<BookStore> GetBookStores();
        List<Author> GetAuthors();
        List<Publisher> GetPublishers();
        Author GetAuthor(int authorId);
        Publisher GetPublisher(int publisherId);

    }
}
