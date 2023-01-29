using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibInterfaceMVVM.Models
{
    public class Book
    {
        public string? CoverURL { get; set; }
        public string? Title { get; set; }
        public string? Author {get; set; }
        public string? YearOfPublishing {get; set; }
        public string? ISBN {get; set; }
    }
}
