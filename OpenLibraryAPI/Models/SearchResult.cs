using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLibraryAPI.Models
{
    public class SearchResult
    {
        public int totalFound;
        public int startFrom;
        public IList<BookDetails>? books;
    }
}
