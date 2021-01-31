using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibraryManagement.Domain.DTO
{
    public class BorrowBookRequestDto
    {
        [Required]
        public int BorrowerId { get; set; }
        [Required]
        public int BookId { get; set; }

    }
}
