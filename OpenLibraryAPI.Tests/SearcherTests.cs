using OpenLibraryAPI;
using Xunit;

namespace OpenLibraryAPI.Tests
{
    public class SearcherTests
    {
        [Fact]
        public async void SearchBook_ReturnBooks()
        {
            //Act
            var result = await OpenLibSearcher.SearchBook("title", "Lord Of The Rings");

            //Assert
            Assert.NotNull(result.books);
            Assert.NotEmpty(result.books);
        }
        
        [Fact]
        public async void SearchBook_ReturnEmptyBooks()
        {
            //Act
            var result = await OpenLibSearcher.SearchBook("title", "qwertyuiopasdfghjkl");

            //Assert
            Assert.NotNull(result.books);
            Assert.Empty(result.books);
        }

        [Fact]
        public async void SearchBook_ReturnLimitedBooks()
        {
            //Arrange
            var limit = 5;

            //Act
            var result = await OpenLibSearcher.SearchBook("title", "Lord Of The Rings",1,limit);

            //Assert
            Assert.NotNull(result.books);
            Assert.Equal(result.books.Count, limit);
        }
        
        [Fact]
        public async void SearchBook_ReturnRightPageOfBooks()
        {
            //Arrange
            var limit = 5;
            var page = 3;
            var startingBook = 10;
            //Act
            var result = await OpenLibSearcher.SearchBook("title", "Lord Of The Rings", page, limit);

            //Assert
            Assert.Equal(result.startFrom, startingBook);
        }

        [Fact]
        public async void SearchByTitle_RetunrBooks()
        {
            //Act
            var result = await OpenLibSearcher.SearchByTitle("Lord Of The Rings");

            //Assert
            Assert.NotNull(result.books);
            Assert.NotEmpty(result.books);
        }

        [Fact]
        public async void SearchByAuthor_RetunrBooks()
        {
            //Act
            var result = await OpenLibSearcher.SearchByAuthor("Dan Brown");

            //Assert
            Assert.NotNull(result.books);
            Assert.NotEmpty(result.books);
        }

        [Fact]
        public async void SmartSearch_RetunrBooks()
        {
            //Act
            var result = await OpenLibSearcher.SmartSearch("Lord Of The Rings");

            //Assert
            Assert.NotNull(result.books);
            Assert.NotEmpty(result.books);
        }
    }
}