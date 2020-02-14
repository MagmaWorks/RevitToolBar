using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarbonCalculator
{
    public class CommandHandlerWithParameter : ICommand
    {
        private Action<object> _action;
        private bool _canExecute;
        public CommandHandlerWithParameter(Action<object> action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}