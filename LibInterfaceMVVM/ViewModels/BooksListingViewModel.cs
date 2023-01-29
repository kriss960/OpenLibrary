using LibInterfaceMVVM.Store;
using OpenLibraryAPI;
using OpenLibraryInterface.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibInterfaceMVVM.ViewModels
{
    public class BooksListingViewModel : BaseViewModel
    {
        private int _results = 0;
        private int _page = 1;
        private bool _isSearching = false;
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private CurrentBookDetailsStore _currentBookDetailsStore;

        public int Results
        {
            get { return _results; }
            set 
            {
                _results = value;
                OnPropertyChanged(nameof(Results));
                OnPropertyChanged(nameof(TotalPages));
                OnPropertyChanged(nameof(ResultsFound));
                OnPropertyChanged(nameof(PageDisplay));
                OnPropertyChanged(nameof(IsNotLastPage));
            }
        }
        public int Page
        {
            get { return _page; }
            set
            {
                _page = value;
                OnPropertyChanged(nameof(Page));
                OnPropertyChanged(nameof(IsNotFirstPage));
                OnPropertyChanged(nameof(IsNotLastPage));
                OnPropertyChanged(nameof(PageDisplay));
            }
        }
        public int ItemsPerPage
        {
            get;
            //set 
            //{
            //    _itemsPerPage = value;
            //    OnPropertyChanged(nameof(ItemsPerPage));
            //    OnPropertyChanged(nameof(TotalPages));
            //    OnPropertyChanged(nameof(PageDisplay));
            //}
        }
        public int TotalPages => (int)Math.Ceiling(_results/(decimal)ItemsPerPage);
        public string ResultsFound => $"{Results} results found.";
        public string PageDisplay => $"Page {Page} of {(TotalPages < 1 ? 1 : TotalPages)}";
        public bool IsNotFirstPage => Page != 1;
        public bool IsNotLastPage => Page < TotalPages;
        public bool IsSearching
        {
            get { return _isSearching; }
            set
            {
                _isSearching = value;
                OnPropertyChanged(nameof(IsSearching));
            }
        }

        public ICommand NextPageCommand { get; }
        public ICommand PrevPageCommand { get; }
        public OpenLibPagination? Searcher { get; set; }


        public ObservableCollection<BooksListingItemViewModel> _booksListingItemViewModels;
        public IList<BooksListingItemViewModel> BooksListingItemViewModels => _booksListingItemViewModels;

        public BooksListingViewModel(int itemsPerPage, CurrentBookDetailsStore currentBookDetailsStore)
        {
            ItemsPerPage = itemsPerPage;
            _booksListingItemViewModels = new ObservableCollection<BooksListingItemViewModel>();
            _currentBookDetailsStore = currentBookDetailsStore;
            NextPageCommand = new RelayCommand(async o => await ChangePageAsync(1));
            PrevPageCommand = new RelayCommand(async o => await ChangePageAsync(-1));
        }

        public async Task ChangePageAsync(int offset)
        {
            if(Searcher is not null && _page + offset > 0 && _page+offset <= TotalPages)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                IsSearching= true;
                Page += offset;

                try
                {
                    var res = await Searcher.GoToPage(Page);
                    Results = res.totalFound;

                    BooksListingItemViewModels.Clear();
                    foreach (var book in res.books!)
                    {
                        BooksListingItemViewModels.Add(new BooksListingItemViewModel(book.Title!, book.Author!, book.CoverId!, book, _currentBookDetailsStore));
                    }
                }catch(Exception ex)
                {
                    logger.Error("Error changing to page {page} of book {query}. {exeption}",Page,Searcher.Query,ex);
                    MessageBox.Show(ex.Message);
                }

                IsSearching = false;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
        }
    }
}
