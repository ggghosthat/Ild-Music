using System;
using System.Windows.Input;

namespace Ild_Music.Command;
public class CommandDelegator : ICommand
{
    public event EventHandler CanExecuteChanged;

    private readonly Action<object> _execute;
    private readonly Predicate<object> _canExecute;


    public CommandDelegator(Action<object> execute, Predicate<object> canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }


    public bool CanExecute(object parameter) => 
        _canExecute == null || _canExecute.Invoke(parameter);

    public void Execute(object parameter) =>
        _execute?.Invoke(parameter);
    
    public void RaiseRaiseCanExecute() => 
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
