using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CRMClientApp.Commands
{
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<object, Task> execute;
        private readonly Func<object, bool> canExecute;
        public AsyncRelayCommand(Func<object, Task> execute, Func<object, bool> canExecute = null) 
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        public async void Execute(object? parameter)
        {
            await execute(parameter);
        }

        public bool CanExecute(object? parameter) =>
            canExecute == null || canExecute(parameter);

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
