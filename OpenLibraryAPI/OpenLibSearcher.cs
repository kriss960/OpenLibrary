using Newtonsoft.Json;
using OpenLibraryAPI.Models;
using System.Collections.Generic;

namespace OpenLibraryAPI
{
    public static class OpenLibSearcher
    {
        private static readonly HttpClient _client = new HttpClient() { BaseAddress = new Uri("https://openlibrary.org") };

        public static async Task<SearchResult> SearchByTitle(string query, int page = 1, int? limit = null)
        {
            return await SearchBook("title",query,page,limit);
        }
        public static async Task<SearchResult> SearchByAuthor(string query, int page = 1, int? limit = null)
        {
            return await SearchBook("author", query, page, limit);
        }
        public static async Task<SearchResult> SmartSearch(string query, int page = 1, int? limit = null)
        {
            return await SearchBook("q", query, page, limit);
        }
                
        public static async Task<SearchResult> SearchBook(string searchBy, string query, int page = 1, int? limit = null)
        {
            var response = await _client.GetAsync($"/search.json?{searchBy}={query}&page={page}{(limit is not null ? $"&limit={limit}":"")}");
            var content = await response.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<dynamic>(content);
            var books = data.docs;

            var result = new SearchResult()
            {
                totalFound = data.numFound,
                startFrom= data.start,
                books = new List<BookDetails>()
            };
            
            foreach (var book in books)
            {
                result.books.Add(new BookDetails
                {
                    Title = book.title is not null ? book.title : "",
                    Author = book.author_name is not null ? book.author_name[0] : "",
                    ISBN = book.isbn is not null ? book.isbn[0] : "",
                    FirstPublishDate = book.first_publish_year is not null ? book.first_publish_year : "",
                    CoverId = book.cover_i is not null ? book.cover_i : ""
                });
            }

            return result;
        }
    }
}