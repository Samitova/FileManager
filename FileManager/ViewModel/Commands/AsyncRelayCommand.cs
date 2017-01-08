using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FileManager.ViewModel.Commands
{
    class AsyncRelayCommand : ICommand, INotifyPropertyChanged
    {
        public event RunWorkerCompletedEventHandler Executed;
        public event RunWorkerCompletedEventHandler Error;
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly BackgroundWorker _backgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true, WorkerReportsProgress = true };
        private readonly Func<bool> _canExecute;

        private int _progressStatus;
        private bool _isCancellationRequested;
        private bool _isExecuting = false;

        public int ProgressStatus
        {
            get { return _progressStatus; }
            set
            {
                _progressStatus = value;
                NotifyPropertyChanged("ProgressStatus");
            }
        }

        /// <summary>
        /// Flag is canceletion requested
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

        public bool CancelIfRequested()
        {
            if (IsCancellationRequested == false)
                return false;

            return true;
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
                    NotifyPropertyChanged("IsExecuting");
                }
            }
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="action"></param>
        /// <param name="canExecute"></param>
        public AsyncRelayCommand(Action<Action<int>, Func<bool>> action, Func<bool> canExecute = null)
        {
            _backgroundWorker.DoWork += (s, e) =>
            {
                IsExecuting = _backgroundWorker.IsBusy;
                IsCancellationRequested = false;
                CommandManager.InvalidateRequerySuggested();
                action(_backgroundWorker.ReportProgress, CancelIfRequested);
            };

            _backgroundWorker.RunWorkerCompleted += (s, e) =>
            {
                IsExecuting = _backgroundWorker.IsBusy;
                if (e.Error == null && Executed != null)
                {
                    OnExecuted(e);
                }

                if (e.Error != null && Error != null)
                {
                    OnError(e);
                }

                CommandManager.InvalidateRequerySuggested();
            };

            _backgroundWorker.ProgressChanged += ProgressChanged;

            _canExecute = canExecute;
        }


        /// <summary>
        /// If can execute
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null
                       ? !_backgroundWorker.IsBusy
                       : !_backgroundWorker.IsBusy && _canExecute();
        }

        /// <summary>
        /// Start action
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            ProgressStatus = 0;
            _backgroundWorker.RunWorkerAsync();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Change progress state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressStatus = e.ProgressPercentage;
        }

        /// <summary>
        /// Call the PropertyChanged event
        /// </summary>
        /// <param name="propertyName">The property name</param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;

            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Invoke Executed event
        /// </summary>
        /// <param name="args"></param>
        protected void OnExecuted(RunWorkerCompletedEventArgs args)
        {
            var executed = Executed;

            if (executed != null)
                executed(this, args);
        }

        /// <summary>
        /// Invoke Error event
        /// </summary>
        /// <param name="args"></param>
        protected void OnError(RunWorkerCompletedEventArgs args)
        {
            var error = Error;

            if (error != null)
                error(this, args);
        }

    }
}
