using LibraryManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Domain.Interfaces.BookBorrower
{
    public interface IBookBorrowerService
    {

        // List<Models.BookStore> GetAllAvailableBooks();
        Book BorrowBook(int bookId, int borrowerId, int quantity, int limit);
        bool ReturnBook(int bookId, int borrowerId, int quantity);
        Borrower GetBorrower(int id);


    }
}
