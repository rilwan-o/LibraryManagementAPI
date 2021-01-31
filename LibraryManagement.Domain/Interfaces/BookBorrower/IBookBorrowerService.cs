using LibraryManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Domain.Interfaces.BookBorrower
{
    public interface IBookBorrowerService
    {

        Book BorrowBook(int bookId, int borrowerId, int quantity, int limit);
        bool ReturnBook(int bookId, int borrowerId, int quantity);
        Borrower GetBorrower(int id);

        int CreateBorrower(string firstName, string lastName, string Email, string password);

        int Login(string email, string password);
        List<BorrowedBook> GetBooksBorrowedByBorrower(int borrowerId);


    }
}
