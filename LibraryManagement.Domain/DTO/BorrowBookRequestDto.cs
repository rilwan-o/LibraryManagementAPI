using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Domain.DTO
{
    public class BorrowBookRequestDto
    {
        public int BorrowerId { get; set; }
        public int BookId { get; set; }

    }
}
