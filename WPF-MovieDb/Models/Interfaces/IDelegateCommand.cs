namespace WPF_MovieDb.Models;

internal interface IDelegateCommand
{
    event EventHandler? CanExecuteChanged;

    bool CanExecute(object? parameter);
    void Execute(object? parameter);
    void RaiseCanExecuteChange();
}
