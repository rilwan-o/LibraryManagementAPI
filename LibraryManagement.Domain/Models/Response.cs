using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Domain.Models
{
    public class Response
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public Object Data { get; set; }
    }
}
