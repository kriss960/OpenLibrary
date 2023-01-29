using LibInterfaceMVVM.Commands;
using LibInterfaceMVVM.Store;
using OpenLibraryAPI.Models;
using OpenLibraryInterface.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LibInterfaceMVVM.ViewModels
{
    public class BooksListingItemViewModel : BaseViewModel
    {
        private readonly BookDetails _book;
        private readonly CurrentBookDetailsStore _currentBookDetailsStore;

        public string Title { get; }
        public string Author { get;  }
        public ICommand ViewDetailsCommand { get; }

        public BooksListingItemViewModel(string title, string author, string bookId,BookDetails bookData, CurrentBookDetailsStore currentBookDetailsStore)
        {
            this.Title = title;
            this.Author = author;
            this._currentBookDetailsStore = currentBookDetailsStore;
            this._book = bookData;
            this.ViewDetailsCommand = new RelayCommand(o => OnDetailsClick());
        }

        public void OnDetailsClick()
        {
            _currentBookDetailsStore.CurrentBook = new Models.Book()
            {
                Title = _book.Title,
                Author = _book.Author,
                ISBN = _book.ISBN,
                YearOfPublishing = _book.FirstPublishDate,
                CoverURL = $"https://covers.openlibrary.org/b/id/{_book.CoverId}-M.jpg"
            };
        }
    }
}
