using LibInterfaceMVVM.Models;
using LibInterfaceMVVM.Store;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibInterfaceMVVM.ViewModels
{
    public class BookDetailsViewModel : BaseViewModel
    {
        private CurrentBookDetailsStore _currentBookDetailsStore;
        private Book? CurrentBook => _currentBookDetailsStore.CurrentBook;
        
        public bool HasDataToDisplay => CurrentBook is not null;
        public string CoverURL => CurrentBook?.CoverURL ?? "";
        public string Title => CurrentBook?.Title ?? "Unknown";
        public string Author => CurrentBook?.Author ?? "";
        public string YearOfPublishing => CurrentBook?.YearOfPublishing ?? "";
        public string ISBN => CurrentBook?.ISBN ?? "";

        public BookDetailsViewModel(CurrentBookDetailsStore currentBookDetailsStore)
        {
            _currentBookDetailsStore = currentBookDetailsStore;
            _currentBookDetailsStore.CurrentBookChanged += _currentBookDetailsStore_CurrentBookChanged;
        }

        protected override void Dispose()
        {
            _currentBookDetailsStore.CurrentBookChanged -= _currentBookDetailsStore_CurrentBookChanged;
        }

        private void _currentBookDetailsStore_CurrentBookChanged()
        {
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Author));
            OnPropertyChanged(nameof(YearOfPublishing));
            OnPropertyChanged(nameof(ISBN));
            OnPropertyChanged(nameof(CoverURL));
            OnPropertyChanged(nameof(HasDataToDisplay));
        }
    }
}
