using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Domain.Models
{
    public class Borrower
    {
        public int Id { get; set; }
        public string UserName  { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateDeleted{ get; set; }
    }
}
