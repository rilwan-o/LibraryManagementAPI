using LibraryManagement.Domain.Interfaces.BookManager;
using LibraryManagement.Domain.Models;
using LibraryManagement.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LibraryManagement.Test
{
    public class BookManagementServiceTest
    {       
        [Fact]
        public void AddAuthor_AddNewAuthor_ReturnsTrue()
        {

            IBookManagementService bookManagementService = new BookManagementService();
            var initialCount = bookManagementService.GetAuthors().Count;
            var firstName = "TesterFirstname1";
            var lastName = "TesterLastName2";
            var actual = bookManagementService.AddAuthor(firstName, lastName);
            var finalCount = bookManagementService.GetAuthors().Count;
            Assert.Equal(initialCount + 1, finalCount);
        }

        [Fact]
        public void AddAuthor_AddExistingAuthor_ThrowsException()
        {

            IBookManagementService bookManagementService = new BookManagementService();
            var initialCount = bookManagementService.GetAuthors().Count;
            var firstName = "John";
            var lastName = "Doe";
            Assert.Throws<Exception>(() => bookManagementService.AddAuthor(firstName, lastName));
            
        }
      
        [Fact]
        public void AddPublisher_AddNewPublisher_ReturnsTrue()
        {
            IBookManagementService bookManagementService = new BookManagementService();
            var initialCount = bookManagementService.GetPublishers().Count;
            var name = "TesterFirstname1";
            var actual = bookManagementService.AddPublisher(name);
            var finalCount = bookManagementService.GetPublishers().Count;
            Assert.Equal(initialCount + 1, finalCount);
        }


        [Fact]
        public void AddPublisher_AddExistingPublisher_ThrowsException()
        {

            IBookManagementService bookManagementService = new BookManagementService();
            var initialCount = bookManagementService.GetPublishers().Count;
            var name = "Wiley";
            Assert.Throws<Exception>(() => bookManagementService.AddPublisher(name));

        }

        [Fact]
        public void AddBookToStore_BookRecordExistButNotInStore_CreatesANewBookRecord()
        {
            IBookManagementService bookManagementService = new BookManagementService();
            var initialCount = bookManagementService.GetBookStores().Count;
            var response = bookManagementService.AddBookToStore(4, 1);
            var finalCount = bookManagementService.GetBookStores().Count;
            Assert.True(response);
            Assert.Equal(initialCount + 1, finalCount);
        }

        [Fact]
        public void AddBookToStore_BookRecordDoesNotExistNotInStore_ThrowsKeyNotFoundException()
        {
            IBookManagementService bookManagementService = new BookManagementService();
            Assert.Throws<KeyNotFoundException>(() => bookManagementService.AddBookToStore(14, 1));
        }

        [Fact]
        public void AddBookToStore_BookRecordExistInStore_ReturnsTrueIncreaseBookQuantityCount()
        {
            IBookManagementService bookManagementService = new BookManagementService();
            var stores = bookManagementService.GetBookStores();            
            var store = stores.FirstOrDefault(b => b.BookId == 1);
            var initialCount = store.Quantity;
            var response = bookManagementService.AddBookToStore(1, 1);
            stores = bookManagementService.GetBookStores();
            store = stores.FirstOrDefault(b => b.BookId == 1);
            var finalCount = store.Quantity;
            Assert.True(response);
            Assert.Equal(initialCount + 1, finalCount);
        }

        [Fact]
        public void RemoveBookFromStore_BookIdExistInBookNotExistInBookStore_ThrowsKeyNotFoundException()
        {
            IBookManagementService bookManagementService = new BookManagementService();
            Assert.Throws<KeyNotFoundException>(() => bookManagementService.RemoveBookFromStore(4, 1));
        }

        [Fact]
        public void RemoveBookFromStore_BookIdExistInBookExistInBookStore_ReturnsTrueQuantityReduces()
        {
            IBookManagementService bookManagementService = new BookManagementService();
            var stores = bookManagementService.GetBookStores();
            var store = stores.FirstOrDefault(b => b.BookId == 1);
            var initialCount = store.Quantity;
            var response = bookManagementService.RemoveBookFromStore(1, 1);
            stores = bookManagementService.GetBookStores();
            store = stores.FirstOrDefault(b => b.BookId == 1);
            var finalCount = store.Quantity;
            Assert.True(response);
            Assert.Equal(initialCount - 1, finalCount);
        }

        [Fact]
        public void RemoveBookFromStore_BookIdExistButQuantityIsLessThanOne_ThrowOutofrangeException()
        {
            IBookManagementService bookManagementService = new BookManagementService();
            var stores = bookManagementService.GetBookStores();
            var store = stores.FirstOrDefault(b => b.BookId == 1);
            var response = bookManagementService.RemoveBookFromStore(1, 1);
            response = bookManagementService.RemoveBookFromStore(1, 1);
            response = bookManagementService.RemoveBookFromStore(1, 1);
            Assert.Throws<ArgumentOutOfRangeException>(() => bookManagementService.RemoveBookFromStore(1, 1));
        }

        [Fact]
        public void RemoveBookFromStore_BookIdExistButRemainingQuantityIsLessThanWantedQuantity_ThrowOutofrangeException()
        {
            IBookManagementService bookManagementService = new BookManagementService();
            var stores = bookManagementService.GetBookStores();
            var store = stores.FirstOrDefault(b => b.BookId == 1);
            var neededQuantity = 20;
            Assert.Throws<ArgumentOutOfRangeException>(() => bookManagementService.RemoveBookFromStore(1, neededQuantity));
        }

        [Fact]
        public void CreateBook_BookAlreadyExist_ThrowException()
        {
            IBookManagementService bookManagementService = new BookManagementService();
            var book = new Book { Id = 2, Name = "English Language", AuthorId = 2, PublisherId = 2, PublishDate = new DateTime(2015, 12, 25) };
            Assert.Throws<Exception>(() => bookManagementService.CreateBook(book));
        }

        [Fact]
        public void CreateBook_BookIsNew_ThrowException()
        {
            IBookManagementService bookManagementService = new BookManagementService();
            var book = new Book { Id=0, Name = "How To Influence and Win People", AuthorId = 2, PublisherId = 2, PublishDate = new DateTime(2020, 12, 25) };
            bookManagementService.CreateBook(book);
            bookManagementService.AddBookToStore(5, 1);
            var stores = bookManagementService.GetBookStores();
            var store = stores.FirstOrDefault(b => b.BookId == 5);
            var initialCount = 0;
            stores = bookManagementService.GetBookStores();
            store = stores.FirstOrDefault(b => b.BookId == 5);
            var finalCount = store.Quantity;
            Assert.Equal(initialCount + 1, finalCount);
        }

    }
}
