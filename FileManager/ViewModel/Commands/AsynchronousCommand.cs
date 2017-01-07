using System.Windows.Input;
using System;
using System.Windows.Threading;
using System.ComponentModel;
using System.Threading;

namespace FileManager.ViewModel.Commands
{
    /// <summary>
    /// The AsynchronousCommand is a Command that runs on a thread from the thread pool.
    /// </summary>
    public class AsynchronousCommand : Command, INotifyPropertyChanged
    {
        protected Dispatcher _callingDispatcher;

        private bool _isExecuting = false;

        private int _progressStatus = 0;

        private bool _isCancellationRequested;

        private Command _cancelCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public event CommandEventHandler Cancelled;

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
                    NotifyPropertyChanged("IsExecuting");
                }
            }
        }

        /// <summary>
        /// Progress status in percents
        /// </summary>
        public int ProgressStatus
        {
            get
            {
                return _progressStatus;
            }
            set
            {
                _progressStatus = value;
                NotifyPropertyChanged("ProgressStatus");
            }
        }

        /// <summary>
        /// Flag if cancelation is requested
        /// </summary>
        public bool IsCancellationRequested
        {
            get
            {
                return _isCancellationRequested;
            }
            set
            {
                if (_isCancellationRequested != value)
                {
                    _isCancellationRequested = value;
                    NotifyPropertyChanged("IsCancellationRequested");
                }
            }
        }

        /// <summary>
        /// Command for cancelation
        /// </summary>
        public Command CancelCommand
        {
            get { return _cancelCommand; }
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="action"></param>
        /// <param name="canExecute"></param>
        public AsynchronousCommand(Action action, bool canExecute = true) : base(action, canExecute)
        {
            Initialise();
        }

        /// <summary>
        /// ctor (action with param)
        /// </summary>
        /// <param name="action"></param>
        /// <param name="canExecute"></param>
        public AsynchronousCommand(Action<object> parameterizedAction, bool canExecute = true) : base(parameterizedAction, canExecute)
        {
            Initialise();
        }

        /// <summary>
        /// Init Cancel command
        /// </summary>
        private void Initialise()
        {
            _cancelCommand = new Command(
              () =>
              {
                  IsCancellationRequested = true;
              }, true);
        }

        /// <summary>
        /// Executes the command on a new thread from the thread pool
        /// </summary>
        /// <param name="param"></param>
        public override void DoExecute(object param)
        {
            ProgressStatus = 0;

            if (IsExecuting)
                return;

            CancelCommandEventArgs args = new CancelCommandEventArgs() { Parameter = param, Cancel = false };
            InvokeExecuting(args);

            if (args.Cancel)
                return;

            IsExecuting = true;

            _callingDispatcher = Dispatcher.CurrentDispatcher;
                     
            ThreadPool.QueueUserWorkItem(
              (state) =>
              {
                  InvokeAction(param);

                  ReportProgress(
                    () =>
                    {                  
                        IsExecuting = false;
                 
                        if (IsCancellationRequested)
                        {
                            InvokeCancelled(new CommandEventArgs() { Parameter = param });
                        }

                        else
                        {
                            InvokeExecuted(new CommandEventArgs() { Parameter = param });
                        }

                        IsCancellationRequested = false;
                    });
              });
        }

        /// <summary>
        /// Call the PropertyChanged event
        /// </summary>
        /// <param name="propertyName">The property name</param>
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Reports progress on the thread which invoked the command
        /// </summary>
        /// <param name="action"></param>
        public void ReportProgress(Action action)
        {
            if (IsExecuting)
            {
                if (_callingDispatcher.CheckAccess())
                    action();
                else
                    _callingDispatcher.BeginInvoke(((Action)(() => { action(); })));
            }
        }

        /// <summary>
        ///  Cancels the command if requested
        /// </summary>
        /// <returns></returns>
        public bool CancelIfRequested()
        {
            if (IsCancellationRequested == false)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Invokes the cancelled event
        /// </summary>
        /// <param name="args"></param>
        protected void InvokeCancelled(CommandEventArgs args)
        {
            CommandEventHandler cancelled = Cancelled;

            if (cancelled != null)
                cancelled(this, args);
        }
    }
}