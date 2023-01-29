using LibInterfaceMVVM.Commands;
using LibInterfaceMVVM.Store;
using NLog;
using OpenLibraryAPI;
using OpenLibraryInterface.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibInterfaceMVVM.ViewModels
{
    public class LibraryViewModel : BaseViewModel
    {
        private bool _searchByTitle = true;
        private bool _searchByAuthor;
        private bool _searchByBoth;
        private bool _isSearching = false;
        private string _searchText = "";
        private readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public bool SearchByTitle
        {
            get { return _searchByTitle; }
            set
            {
                _searchByTitle = value;
                OnPropertyChanged(nameof(SearchByTitle));
                OnPropertyChanged(nameof(SearchByAuthor));
                OnPropertyChanged(nameof(SearchByBoth));
                OnPropertyChanged(nameof(CanSearch));
            }
        }
        public bool SearchByAuthor
        {
            get { return _searchByAuthor; }
            set
            {
                _searchByAuthor = value;
                OnPropertyChanged(nameof(SearchByTitle));
                OnPropertyChanged(nameof(SearchByAuthor));
                OnPropertyChanged(nameof(SearchByBoth));
            }
        }
        public bool SearchByBoth
        {
            get { return _searchByBoth; }
            set
            {
                _searchByBoth = value;
                OnPropertyChanged(nameof(SearchByTitle));
                OnPropertyChanged(nameof(SearchByAuthor));
                OnPropertyChanged(nameof(SearchByBoth));
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                OnPropertyChanged(nameof(CanSearch));
            }
        }

        public BooksListingViewModel BookListingViewModel { get; }
        public BookDetailsViewModel BookDetailsViewModel { get; }

        public ICommand SearchBooksCommand { get; }
        public bool IsSearching { get { return _isSearching; }
            set
            {
                _isSearching = value;
                OnPropertyChanged(nameof(IsSearching));
                OnPropertyChanged(nameof(IsControlsEnabled));
            }
        }
        public bool CanSearch => !string.IsNullOrEmpty(_searchText) && (SearchByTitle || SearchByAuthor || SearchByBoth);
        public bool IsControlsEnabled => !IsSearching && !BookListingViewModel.IsSearching;
        public LibraryViewModel(CurrentBookDetailsStore _currentBookDetailsStore)
        {
            BookListingViewModel = new BooksListingViewModel(5, _currentBookDetailsStore);
            BookDetailsViewModel = new BookDetailsViewModel(_currentBookDetailsStore);
            SearchBooksCommand = new RelayCommand(async o => await OnSearchButtonAsync(_currentBookDetailsStore));
            BookListingViewModel.PropertyChanged += BookListingViewModel_PropertyChanged;
        }

        private void BookListingViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName== nameof(IsSearching)) { OnPropertyChanged(nameof(IsControlsEnabled)); }
        }

        public async Task OnSearchButtonAsync(CurrentBookDetailsStore _currentBookDetailsStore)
        {
            IsSearching= true;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            BookListingViewModel.BooksListingItemViewModels.Clear();
            OpenLibPagination s;
            if (SearchByTitle)
            {
                s = new OpenLibPagination(SearchText, 5, OpenLibSearcher.SearchByTitle);
                Logger.Info("Searching book by {criteria} with {query}","title",SearchText);
            }
            else if (SearchByAuthor)
            {
                s = new OpenLibPagination(SearchText, 5, OpenLibSearcher.SearchByAuthor);
                Logger.Info("Searching book by {criteria} with {query}", "author", SearchText);
            }
            else
            {
                s = new OpenLibPagination(SearchText, 5, OpenLibSearcher.SmartSearch);
                Logger.Info("Searching book by {criteria} with {query}", "smart search", SearchText);
            }
            try
            {
                var res = await s.SearchResults();
                BookListingViewModel.Searcher = s;
                BookListingViewModel.Page = 1;
                BookListingViewModel.Results = res.totalFound;
                Logger.Info("{count} books were found", res.totalFound);
                foreach (var book in res.books!)
                {
                    BookListingViewModel.BooksListingItemViewModels.Add(new BooksListingItemViewModel(book.Title!, book.Author!, book.CoverId!,book, _currentBookDetailsStore));
                }
            }catch(Exception ex)
            {
                Logger.Error("Error while searching book {query} - {error}", SearchText, ex.Message);
                MessageBox.Show(ex.Message);
            }

            IsSearching = false;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }
    }
}
