using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FileManager.ViewModel.Commands
{
    public class RelayCommand : ICommand, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;        

        private bool _isExecuting = false;

        /// <summary>
        /// Occurs when the command is about to execute
        /// </summary>
        public event EventHandler Executing;

        /// <summary>
        /// Occurs when the command executed
        /// </summary>
        public event EventHandler Executed;

        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action<object> execute) : this(execute, null)
        { }

        public RelayCommand(Action execute) : this(o => execute())
        { }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }                

            _execute = execute;
            _canExecute = canExecute;
        }
        
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            IsExecuting = true;
            OnExecuting(new EventArgs());

            _execute(parameter);

            IsExecuting = false;

            OnExecuted(new EventArgs());
        }
               

        /// <summary>
        /// Flag is command executing
        /// </summary>
        public bool IsExecuting
        {
            get
            {
                return _isExecuting;
            }
            set
            {
                if (_isExecuting != value)
                {
                    _isExecuting = value;
                    OnPropertyChanged("IsExecuting");
                }
            }
        }

        /// <summary>
        /// Call the Executed event
        /// </summary>
        /// <param name="args"></param>
        protected void OnExecuted(EventArgs args)
        {
            Executed?.Invoke(this, args);
        }

        /// <summary>
        /// Call the Executing event
        /// </summary>
        /// <param name="args"></param>
        protected void OnExecuting(EventArgs args)
        {
            Executing?.Invoke(this, args);           
        }

        /// <summary>
        /// Call the PropertyChanged event
        /// </summary>
        /// <param name="propertyName">The property name</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
