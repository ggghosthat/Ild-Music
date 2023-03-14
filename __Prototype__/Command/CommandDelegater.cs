using System;
using System.Windows.Input;

namespace Ild_Music_MVVM_.Command
{
    public class CommandDelegater : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly Action<object> _open;
        private readonly Predicate<object> _canExecute;

        public CommandDelegater(Action<object> open, Predicate<object> canExecute = null)
        {
            _open = open;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => 
            _canExecute == null || _canExecute.Invoke(parameter);

        public void Execute(object parameter) =>
            _open?.Invoke(parameter);
        
        public void RaiseRaiseCanExecute() => 
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        
    }
}
