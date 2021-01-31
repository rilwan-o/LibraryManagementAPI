using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Domain.Models
{
    public class BorrowedBook
    {
        public int Id { get; set; }
        public int? BookId { get; set; }
        public int? BorrowerId { get; set; }
        public int Status { get; set; }
        public DateTime DateBorrowed { get; set; }
        public DateTime? DateReturned { get; set; }
    }
}
