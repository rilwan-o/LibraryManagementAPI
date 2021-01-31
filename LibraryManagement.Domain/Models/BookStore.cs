using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Domain.Models
{
    public class BookStore
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
    }
}
