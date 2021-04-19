using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PicoShelter_DesktopApp.Commands
{
    public abstract class AsyncCommandBase : ICommand
    {
        private readonly Action<Exception> _onException;
        private readonly Func<bool> _canExecuteAdditional;

        private bool _isExecuting;
        public bool IsExecuting
        {
            get
            {
                return _isExecuting;
            }
            set
            {
                _isExecuting = value;
                _internalCanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }

        public event EventHandler _internalCanExecuteChanged;

        public event EventHandler CanExecuteChanged
        {
            add 
            {
                _internalCanExecuteChanged += value;
                CommandManager.RequerySuggested += value; 
            }
            remove 
            {
                _internalCanExecuteChanged -= value;
                CommandManager.RequerySuggested -= value; 
            }
        }

        public AsyncCommandBase(Action<Exception> onException, Func<bool> canExecuteAdditional = null)
        {
            _onException = onException;
            _canExecuteAdditional = canExecuteAdditional;
        }

        public bool CanExecute(object parameter)
        {
            var canExec = !IsExecuting;

            if (_canExecuteAdditional != null)
                canExec = canExec && _canExecuteAdditional();

            return canExec;
        }

        public async void Execute(object parameter)
        {
            IsExecuting = true;

            try
            {
                await ExecuteAsync(parameter);
            }
            catch (Exception ex)
            {
                _onException?.Invoke(ex);
            }

            IsExecuting = false;
        }

        protected abstract Task ExecuteAsync(object parameter);
    }
}
