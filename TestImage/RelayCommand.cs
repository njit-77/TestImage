using System;
using System.Diagnostics;
using System.Windows.Input;

namespace TestImage
{
    public class RelayCommand<T> : ICommand
    {

        #region 构造函数

        public RelayCommand(Action<T> execute) : this(execute, null)
        {
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion


        #region 字段

        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;

        #endregion


        #region ICommand的成员

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        [DebuggerStepThrough]
        public Boolean CanExecute(Object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }
            return _canExecute((T)parameter);
        }

        public void Execute(Object parameter)
        {
            if (_execute != null)
            {
                _execute((T)parameter);
            }
        }

        #endregion

    }

    public class RelayCommand : ICommand
    {

        #region 构造函数

        public RelayCommand(Action execute) : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Func<Boolean> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion


        #region 字段

        private readonly Func<Boolean> _canExecute;
        private readonly Action _execute;

        #endregion


        #region ICommand的成员

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        [DebuggerStepThrough]
        public Boolean CanExecute(Object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }
            return _canExecute();
        }

        public void Execute(Object parameter)
        {
            if (_execute != null)
            {
                _execute();
            }
        }

        #endregion

    }
}
