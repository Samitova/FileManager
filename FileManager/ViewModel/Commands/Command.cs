using System.Windows.Input;
using System;

namespace FileManager.ViewModel.Commands
{
    public delegate void CommandEventHandler(object sender, CommandEventArgs args);

    public delegate void CancelCommandEventHandler(object sender, CancelCommandEventArgs args);

    /// <summary>
    /// Holds the command parameter
    /// </summary>
    public class CommandEventArgs : EventArgs
    {
        public object Parameter { get; set; }
    }

    /// <summary>
    /// Allows the event to be cancelled.
    /// </summary>
    public class CancelCommandEventArgs : CommandEventArgs
    {
        public bool Cancel { get; set; }
    }



    public class Command : ICommand
    {
        protected Action _action = null;
        protected Action<object> _parameterizedAction = null;
        private bool _canExecute = false;

        /// <summary>
        /// Occurs when canExecute is changed
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Occurs when the command is executing
        /// </summary>
        public event CancelCommandEventHandler Executing;

        /// <summary>
        /// Occurs when the command executed
        /// </summary>
        public event CommandEventHandler Executed;             

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="action"></param>
        /// <param name="canExecute"></param>
        public Command(Action action, bool canExecute = true)
        {
            _action = action;
            _canExecute = canExecute;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="parameterizedAction"></param>
        /// <param name="canExecute"></param>
        public Command(Action<object> parameterizedAction, bool canExecute = true)
        {
            _parameterizedAction = parameterizedAction;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Execute command
        /// </summary>
        /// <param name="param"></param>
        public virtual void DoExecute(object param)
        {
            CancelCommandEventArgs args = new CancelCommandEventArgs() { Parameter = param, Cancel = false };
            InvokeExecuting(args);

            if (args.Cancel)
                return;

            InvokeAction(param);

            InvokeExecuted(new CommandEventArgs() { Parameter = param });
        }

        /// <summary>
        /// Call the action or the parameterized action, whichever has been set
        /// </summary>
        /// <param name="param"></param>
        protected void InvokeAction(object param)
        {
            Action theAction = _action;
            Action<object> theParameterizedAction = _parameterizedAction;
            if (theAction != null)
                theAction();
            else if (theParameterizedAction != null)
                theParameterizedAction(param);
        }

        /// <summary>
        /// Call the executed function
        /// </summary>
        /// <param name="args"></param>
        protected void InvokeExecuted(CommandEventArgs args)
        {
            CommandEventHandler executed = Executed;

            if (executed != null)
                executed(this, args);
        }

        /// <summary>
        /// Call the executing function
        /// </summary>
        /// <param name="args"></param>
        protected void InvokeExecuting(CancelCommandEventArgs args)
        {
            CancelCommandEventHandler executing = Executing;

            if (executing != null)
                executing(this, args);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can execute.
        /// </summary>
        /// <value> </value>
        public bool CanExecute
        {
            get { return _canExecute; }
            set
            {
                if (_canExecute != value)
                {
                    _canExecute = value;
                    EventHandler canExecuteChanged = CanExecuteChanged;
                    if (canExecuteChanged != null)
                        canExecuteChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool ICommand.CanExecute(object parameter)
        {
            return _canExecute;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter"></param>
        void ICommand.Execute(object parameter)
        {
            DoExecute(parameter);
        }
    }

}