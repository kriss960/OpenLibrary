using OpenLibraryAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLibraryAPI
{
    public class OpenLibPagination 
    {
        private readonly Func<string,int,int?,Task<SearchResult>> searcher;
        private readonly int itemsPerPage = 1;

        public OpenLibPagination(string query, int itemsPerPage, Func<string, int, int?, Task<SearchResult>> searcher)
        {
            this.itemsPerPage = itemsPerPage > 0 ? itemsPerPage : 1;
            Query = query;
            this.searcher = searcher;
        }

        public string Query { get; }
        public int Page { get; private set; } 
        public int PageCount { get; private set; }
        public int TotalCount { get; private set; }

        public async Task<SearchResult> SearchResults()
        {
            Page = 1;
            var res = await searcher(Query, Page, itemsPerPage);
            TotalCount = res.totalFound;
            PageCount = TotalCount / itemsPerPage;
            return res;
        }
        public async Task<SearchResult> NextPage()
        {
            return await GoToPage(Page + 1);
        }
        public async Task<SearchResult> PrevPage()
        {
            return await GoToPage(Page - 1);
        }
        public async Task<SearchResult> GoToPage(int page)
        {
            if (page < 1 || page > PageCount) { throw new IndexOutOfRangeException("Selected page is invalid"); }
            Page = page;
            return await searcher(Query, Page, itemsPerPage);
        }
    }
}
