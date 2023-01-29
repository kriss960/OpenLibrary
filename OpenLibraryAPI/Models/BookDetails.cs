using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLibraryAPI.Models
{
    public class BookDetails
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? ISBN { get; set; }
        public string? FirstPublishDate { get; set; }
        public string? CoverId { get; set; }
    }
}
