using System;
using System.Windows.Input;

namespace CulturalSiberiaDiplom.Services;

public class NavigateCommand : ICommand
{
    private readonly Action<string, string, object> _executeAction;

    public NavigateCommand(Action<string, string, object> executeAction)
    {
        _executeAction = executeAction;
    }

    public bool CanExecute(object? parametr)
    {
        return true;
    }

    public void Execute(object? parametr)
    {
        var args = (Tuple<string, string, object>)parametr!;
        _executeAction(args.Item1, args.Item2, args.Item3);
    }

    public event EventHandler CanExecuteChanged;
}