using LibraryManagement.Domain.Interfaces.BookBorrower;
using LibraryManagement.Domain.Interfaces.BookManager;
using LibraryManagement.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LibraryManagement.Test
{
    public class BookBorrowerServiceTest
    {
        [Fact]
        public void BorrowBook_BorrowerIdDoesNotExist_ThrowKeyNotFoundException()
        {
            IBookManagementService bookManagementService = new BookManagementService();
            IBookBorrowerService bookBorrowerService = new BookBorrowerService(bookManagementService);
            int borrowerId = 20;
            int bookId = 1;
            int quantity = 1;
            int limit = 2;
            
            Assert.Throws<KeyNotFoundException>(() => bookBorrowerService.BorrowBook(bookId, borrowerId, quantity, limit));
        }

        [Fact]
        public void BorrowBook_BorrowerHasBorrowedUpToLimit_ThrowInvalidOperationException()
        {
            IBookManagementService bookManagementService = new BookManagementService();
            IBookBorrowerService bookBorrowerService = new BookBorrowerService(bookManagementService);
            int borrowerId = 1;
            int bookId = 1;
            int quantity = 1;
            int limit = 2;
            bookBorrowerService.BorrowBook(bookId, borrowerId, quantity, limit);
            bookId = 2;
            bookBorrowerService.BorrowBook(bookId, borrowerId, quantity, limit);
            bookId = 3;
            Assert.Throws<InvalidOperationException>(() => bookBorrowerService.BorrowBook(bookId, borrowerId, quantity, limit));
        }

        [Fact]
        public void BorrowBook_BookHasAlreadyBeenBorrowedBySamePerson_ThrowInvalidOperationException()
        {
            IBookManagementService bookManagementService = new BookManagementService();
            IBookBorrowerService bookBorrowerService = new BookBorrowerService(bookManagementService);
            int borrowerId = 1;
            int bookId = 1;
            int quantity = 1;
            int limit = 2;
            bookBorrowerService.BorrowBook(bookId, borrowerId, quantity, 1);

            Assert.Throws<InvalidOperationException>(() => bookBorrowerService.BorrowBook(bookId, borrowerId, quantity, limit));
        }

        [Fact]
        public void BorrowBook_BookBorrowedOnceByDifferentPersons_QuntityReducesByTwo()
        {
            IBookManagementService bookManagementService = new BookManagementService();
            IBookBorrowerService bookBorrowerService = new BookBorrowerService(bookManagementService);
            int bookId = 1;
            var stores = bookManagementService.GetBookStores();
            var store = stores.FirstOrDefault(b => b.BookId == bookId);
            var initialCount = store.Quantity;
            int borrowerId= 1;            
            int quantity = 1;
            int limit = 2;
            bookBorrowerService.BorrowBook(bookId, borrowerId, quantity, limit);
            borrowerId = 2;
            bookBorrowerService.BorrowBook(bookId, borrowerId, quantity, limit);

            stores = bookManagementService.GetBookStores();
            store = stores.FirstOrDefault(b => b.BookId == bookId);
            var finalCount = store.Quantity;
            Assert.Equal(initialCount - 2, finalCount);
        }

        [Fact]
        public void ReturnBook_BookWasNotInitiallyBorrowed_ThrowException()
        {
            IBookManagementService bookManagementService = new BookManagementService();
            IBookBorrowerService bookBorrowerService = new BookBorrowerService(bookManagementService);
            int borrowerId = 1;
            int bookId = 1;
            int quantity = 1;

            Assert.Throws<KeyNotFoundException>(() => bookBorrowerService.ReturnBook(bookId, borrowerId, quantity));
        }

        [Fact]
        public void ReturnBook_BookWasInitiallyBorrowed_ReturnsTrue()
        {
            IBookManagementService bookManagementService = new BookManagementService();
            IBookBorrowerService bookBorrowerService = new BookBorrowerService(bookManagementService);
            int bookId = 1;
            var stores = bookManagementService.GetBookStores();
            var store = stores.FirstOrDefault(b => b.BookId == bookId);
            var initialCount = store.Quantity;
            int borrowerId = 1;
            int quantity = 1;
            bookBorrowerService.BorrowBook(bookId, borrowerId, quantity, 1);
            bookBorrowerService.ReturnBook(bookId, borrowerId, quantity);
            stores = bookManagementService.GetBookStores();
            store = stores.FirstOrDefault(b => b.BookId == bookId);
            var finalCount = store.Quantity;

            Assert.Equal(initialCount, finalCount);
        }
        [Fact]
        public void CreateBorrower_NewBorrowerDifferentUsername_ReturnsTrue()
        {
            IBookManagementService bookManagementService = new BookManagementService();
            IBookBorrowerService bookBorrowerService = new BookBorrowerService(bookManagementService);

            var response = bookBorrowerService.CreateBorrower("Jide", "Balogun", "Jido", "dfghjk");
            var borrower = bookBorrowerService.GetBorrower(response);
            Assert.True(borrower.FirstName== "Jide" && borrower.LastName=="Balogun" && borrower.Email=="Jido");



        }

        [Fact]
        public void CreateBorrower_BorrowerExistwithSameUsername_ThrowsException()
        {
            IBookManagementService bookManagementService = new BookManagementService();
            IBookBorrowerService bookBorrowerService = new BookBorrowerService(bookManagementService);

            bookBorrowerService.CreateBorrower("Jide", "Balogun", "Jido", "cgvghjk");
            Assert.Throws<InvalidOperationException>(() => bookBorrowerService.CreateBorrower("Wale", "Jerry", "Jido", "iouytr"));
           
        }
    }
}
