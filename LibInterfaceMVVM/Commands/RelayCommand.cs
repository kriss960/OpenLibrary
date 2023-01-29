using LibInterfaceMVVM.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OpenLibraryInterface.Commands
{
    public class RelayCommand : BaseCommand
    {
        private readonly Action<object> _execute;

        public RelayCommand(Action<object> execute)
        {
            if (execute == null) throw new ArgumentNullException("execute");

            _execute = execute;
        }

        public override void Execute(object? parameter)
        {
            _execute(parameter ?? "<N/A>");
        }
    }
}
