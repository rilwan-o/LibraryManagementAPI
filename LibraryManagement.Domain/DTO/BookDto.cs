using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Domain.DTO
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public DateTime PublishedDate { get; set; }
        public int Quantity { get; set; }
    }
}
