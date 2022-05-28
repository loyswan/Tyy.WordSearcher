using System;

#pragma warning disable SA1512

namespace Tyy.WordSearcher.Mvvm
{
    public sealed class RelayCommand : IRelayCommand
    {

        private readonly Action execute;


        private readonly Func<bool> canExecute;


        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action execute)
        {
            this.execute = execute;
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }


        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }


        public bool CanExecute(object parameter)
        {
            return this.canExecute?.Invoke() != false;
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                this.execute();
            }
        }
    }


    public sealed class RelayCommand<T> : IRelayCommand<T>
    {

        private readonly Action<T> execute;


        private readonly Predicate<T> canExecute;


        public event EventHandler CanExecuteChanged;


        public RelayCommand(Action<T> execute)
        {
            this.execute = execute;
        }


        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }


        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }


        public bool CanExecute(T parameter)
        {
            return this.canExecute?.Invoke(parameter) != false;
        }

        public bool CanExecute(object parameter)
        {
            if (default(T) != null && parameter == null)
            {
                return false;
            }

            return CanExecute((T)parameter);
        }

        public void Execute(T parameter)
        {
            if (CanExecute(parameter))
            {
                this.execute(parameter);
            }
        }

        public void Execute(object parameter)
        {
            Execute((T)parameter);
        }
    }
}