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

        protected Dispatcher callingDispatcher;

        private bool isExecuting = false;

        private int progressStatus = 0;

        private bool isCancellationRequested;

        private Command cancelCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public event CommandEventHandler Cancelled;

        public bool IsExecuting
        {
            get
            {
                return isExecuting;
            }
            set
            {
                if (isExecuting != value)
                {
                    isExecuting = value;
                    NotifyPropertyChanged("IsExecuting");
                }
            }
        }

        public int ProgressStatus
        {
            get
            {
                return progressStatus;
            }
            set
            {
                progressStatus = value;
                NotifyPropertyChanged("ProgressStatus");
            }
        }

        public bool IsCancellationRequested
        {
            get
            {
                return isCancellationRequested;
            }
            set
            {
                if (isCancellationRequested != value)
                {
                    isCancellationRequested = value;
                    NotifyPropertyChanged("IsCancellationRequested");
                }
            }
        }

        public Command CancelCommand
        {
            get { return cancelCommand; }
        }

        /// <summary>
        /// ctor
        /// </summary>       
        public AsynchronousCommand()
        {}

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="action"></param>
        /// <param name="canExecute"></param>
        public AsynchronousCommand(Action action, bool canExecute = true) : base(action, canExecute)
        {
            Initialise();
        }

        public AsynchronousCommand(Action<object> parameterizedAction, bool canExecute = true) : base(parameterizedAction, canExecute)
        {
            Initialise();
        }

        private void Initialise()
        {
            cancelCommand = new Command(
              () =>
              {
                  IsCancellationRequested = true;
              }, true);
        }

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

            callingDispatcher = Dispatcher.CurrentDispatcher;
                     
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
               
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
               
        public void ReportProgress(Action action)
        {
            if (IsExecuting)
            {
                if (callingDispatcher.CheckAccess())
                    action();
                else
                    callingDispatcher.BeginInvoke(((Action)(() => { action(); })));
            }
        }
      
        public bool CancelIfRequested()
        {
            if (IsCancellationRequested == false)
            {
                return false;
            }

            return true;
        }
 
        protected void InvokeCancelled(CommandEventArgs args)
        {
            CommandEventHandler cancelled = Cancelled;

            if (cancelled != null)
                cancelled(this, args);
        }

    }
}