using LibraryManagement.Domain.Interfaces.BookBorrower;
using LibraryManagement.Domain.Interfaces.BookManager;
using LibraryManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryManagement.Domain.Services
{
    public class BookBorrowerService : IBookBorrowerService
    {
        private readonly List<Borrower> borrowers;
        private readonly List<BorrowedBook> borrowedBooks;
        private readonly IBookManagementService _bookManagementService;

        public BookBorrowerService(IBookManagementService bookManagementService)
        {
            _bookManagementService = bookManagementService;

            borrowers = new List<Borrower>() {
                new Borrower{Id=1, Email="lampard", FirstName="Lambert", LastName="Paul", Password="pass",  DateCreated=new DateTime(2015, 12, 25) },
                new Borrower{Id=2, Email="Jerry", FirstName="Joe", LastName="Ibrahim", Password="pass",  DateCreated=new DateTime(2017, 12, 25) },
                new Borrower{Id=3, Email="Willy", FirstName="Zambi", LastName="Wiza", Password="pass", DateCreated=new DateTime(2018, 12, 25) },
                new Borrower{Id=4, Email="Hamed", FirstName="Dennis", LastName="Michael", Password="pass", DateCreated=new DateTime(2019, 12, 25) },

            };

            borrowedBooks = new List<BorrowedBook>();
        }

        public Borrower GetBorrower(int id)
        {
            var borrower = borrowers.FirstOrDefault(b => b.Id == id);
            return borrower;
        }
        public Book BorrowBook(int bookId, int borrowerId, int quantity, int limit)
        {
            CheckBorrowerIdExist(borrowerId);
            var book = _bookManagementService.GetBook(bookId);
            if(book ==null) throw new KeyNotFoundException("Book Id not found");
            var status = 1;
            var booksBorrwedByBorrower = borrowedBooks.FindAll(b => b.BorrowerId == borrowerId && b.Status == status);
            if (booksBorrwedByBorrower.Count >= limit) throw new InvalidOperationException($"Maximum borrowing Limit of {limit} exceeded by borrower");

            var bookAlreadyBorrowed = borrowedBooks.FirstOrDefault(b => b.BookId == bookId && b.BorrowerId == borrowerId && b.Status == 1);
            if (bookAlreadyBorrowed != null) throw new InvalidOperationException("A copy of the book has been already borrowed by this borrower");

            _bookManagementService.RemoveBookFromStore(bookId, quantity);
            var borrowedbook = new BorrowedBook { BookId = bookId, BorrowerId = borrowerId, Status = status, DateBorrowed = DateTime.Now };
            borrowedbook.Id = borrowedBooks.Count + 1;
            borrowedBooks.Add(borrowedbook);
            return book;
        }

        private void CheckBorrowerIdExist(int borrowerId)
        {
            var borrower = borrowers.FirstOrDefault(b => b.Id == borrowerId);
            if (borrower == null) throw new KeyNotFoundException("Borrower Id not found");
        }

        public bool ReturnBook(int bookId, int borrowerId, int quantity)
        {
            CheckBorrowerIdExist(borrowerId);
            var status = 1;

            var borrowedbook = borrowedBooks.FirstOrDefault(b => b.BookId == bookId && b.BorrowerId == borrowerId && b.Status == status);
            if (borrowedbook == null) throw new KeyNotFoundException("Transaction not found");

            var resp = _bookManagementService.AddBookToStore(bookId, quantity);
            if (resp)
            {
                borrowedbook.Status = 0;
                borrowedbook.DateReturned = DateTime.Now;
            }
            return resp;
        }

        public int CreateBorrower(string firstName, string lastName, string email, string password)
        {
            var existingBorrower = borrowers.FirstOrDefault(b => b.Email == email);
            if(existingBorrower != null) throw new InvalidOperationException("A borrower with same Email already exists");
            var borrower = new Borrower { FirstName = firstName, LastName = lastName, Email = email, Password = password, DateCreated = DateTime.Now, DateDeleted = null };
            borrower.Id = borrowers.Count + 1;
            borrowers.Add(borrower);
            return borrower.Id;

        }

        public int Login(string email, string password)
        {
            var existingBorrower = borrowers.FirstOrDefault(b => b.Email.ToLower() == email.ToLower() && b.Password == password);
            if (existingBorrower != null) return existingBorrower.Id;
            return 0;
        }

        public List<BorrowedBook> GetBooksBorrowedByBorrower(int borrowerId)
        {
            CheckBorrowerIdExist(borrowerId);
            return borrowedBooks.Where(b => b.BorrowerId == borrowerId && b.Status == 1).ToList();

        }
    }
}
