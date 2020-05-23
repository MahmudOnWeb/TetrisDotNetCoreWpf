using System;
using System.Windows.Input;

namespace NetCoreTetris
{
    public class SimpleCommand : ICommand
    {
        private readonly Action<object> e;
        private readonly Func<object, bool> ce;

        public SimpleCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            if (execute == null)
            {
#pragma warning disable IDE0016 // Use 'throw' expression
                throw new ArgumentNullException("execute", "Action argument cannot be null");
#pragma warning restore IDE0016 // Use 'throw' expression
            }

            e = execute;
            ce = canExecute;
        }

        public SimpleCommand(Action<object> execute) : this(execute, null)
        {

        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            if (ce == null)
            {
                return true;
            }

            return ce(parameter);
        }

        public void Execute(object parameter)
        {
            e(parameter);
        }
    }
}
