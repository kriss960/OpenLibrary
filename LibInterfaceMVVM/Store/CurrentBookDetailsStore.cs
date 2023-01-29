using LibInterfaceMVVM.Models;
using OpenLibraryAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibInterfaceMVVM.Store
{
    public class CurrentBookDetailsStore
    {
		private Book? _currentBook;

		public Book? CurrentBook
		{
			get { return _currentBook; }
			set 
			{
				_currentBook = value;
				CurrentBookChanged?.Invoke();
			}
		}

		public event Action? CurrentBookChanged;
	}
}
