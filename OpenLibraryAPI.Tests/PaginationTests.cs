using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OpenLibraryAPI.Tests
{
    public class PaginationTests
    {
        [Fact]
        public async void SearchResults_ReturnBooks()
        {
            //Arrange
            var limit = 5;
            OpenLibPagination pagination = new OpenLibPagination("Lord Of The Rings", limit, OpenLibSearcher.SearchByTitle);

            //Act
            var result = await pagination.SearchResults();

            //Assert
            Assert.NotNull(result.books);
            Assert.Equal(limit, result.books.Count);
        }

        [Fact]
        public async void SearchResults_ReturnEmptyBooks()
        {
            //Arrange
            var limit = 5;
            OpenLibPagination pagination = new OpenLibPagination("Lqwertyuiopasdfghjkl", limit, OpenLibSearcher.SearchByTitle);

            //Act
            var result = await pagination.SearchResults();

            //Assert
            Assert.NotNull(result.books);
            Assert.Empty(result.books);
        }

        [Fact]
        public async void NextPage_ReturnNextPageBooks()
        {
            //Arrange
            var limit = 5;
            var nextPage = 2;
            OpenLibPagination pagination = new OpenLibPagination("Lord Of The Rings", limit, OpenLibSearcher.SearchByTitle);

            //Act
            var result = await pagination.SearchResults();
            result = await pagination.NextPage();

            //Assert
            Assert.NotNull(result.books);
            Assert.Equal(limit, result.startFrom);
            Assert.Equal(nextPage, pagination.Page);
        }

        [Fact]
        public async void GoToPage_ReturnRightPageBooks()
        {
            //Arrange
            var limit = 5;
            var nextPage = 4;
            OpenLibPagination pagination = new OpenLibPagination("Lord Of The Rings", limit, OpenLibSearcher.SearchByTitle);

            //Act
            var result = await pagination.SearchResults();
            result = await pagination.GoToPage(nextPage);

            //Assert
            Assert.NotNull(result.books);
            Assert.Equal(limit * nextPage - limit, result.startFrom);
            Assert.Equal(nextPage, pagination.Page);
        }

        [Fact]
        public async void GoToPage_ThrowsForTooSmallPage()
        {
            //Arrange
            var limit = 5;
            var nextPage = -1;
            OpenLibPagination pagination = new OpenLibPagination("Lord Of The Rings", limit, OpenLibSearcher.SearchByTitle);

            //Act
            var result = await pagination.SearchResults();

            //Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await pagination.GoToPage(nextPage));
        }

        [Fact]
        public async void GoToPage_ThrowsForTooLargePage()
        {
            //Arrange
            var limit = 5;
            OpenLibPagination pagination = new OpenLibPagination("Lord Of The Rings", limit, OpenLibSearcher.SearchByTitle);

            //Act
            var result = await pagination.SearchResults();
            var nextPage = pagination.PageCount + 1;

            //Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await pagination.GoToPage(nextPage));
        }

        [Fact]
        public async void PrevPage_ReturnsPrevPageBooks()
        {
            //Arrange
            var limit = 5;
            var previousPage = 2;
            OpenLibPagination pagination = new OpenLibPagination("Lord Of The Rings", limit, OpenLibSearcher.SearchByTitle);

            //Act
            _ = await pagination.SearchResults();
            _ = await pagination.GoToPage(3);
            var result = await pagination.PrevPage();

            //Assert
            Assert.NotNull(result.books);
            Assert.Equal(limit, result.startFrom);
            Assert.Equal(previousPage, pagination.Page);
        }
    }
}
