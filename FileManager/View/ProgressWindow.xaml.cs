using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FileManager.View
{
    public partial class ProgressWindow : Window
    {
        public ProgressWindow()
        {
            InitializeComponent();
        }

             //    private readonly BackgroundWorker _worker;

        //    /// <summary>
        //    /// The timer to be used for automatic progress bar updated.
        //    /// </summary>
        //    private readonly DispatcherTimer _progressTimer;

        //    /// <summary>
        //    /// If set, the interval in which the progress bar
        //    /// gets incremented automatically.
        //    /// </summary>
        //    private int? _autoIncrementInterval;

        //    /// <summary>
        //    /// Defines the size of a single increment of the progress bar.
        //    /// Defaults to 5.
        //    /// </summary>
        //    private int _progressBarIncrement = 5;

        //    private DoWorkEventHandler _workerCallback;

        //    /// <summary>
        //    /// Whether the process was cancelled by the user.
        //    /// </summary>
        //    public bool Cancelled { get; private set; }

        //    /// <summary>
        //    /// If set, the interval in which the progress bar
        //    /// gets incremented automatically.
        //    /// </summary>
        //    /// <exception cref="ArgumentOutOfRangeException">If the interval
        //    /// is lower than 100 ms.</exception>
        //    public int? AutoIncrementInterval
        //    {
        //        get { return _autoIncrementInterval; }
        //        set
        //        {
        //            if (value.HasValue && value < 100) throw new ArgumentOutOfRangeException("value");
        //            _autoIncrementInterval = value;
        //        }
        //    }
        //    /// <summary>
        //    /// Gets or sets the dialog text.
        //    /// </summary>
        //    public string DialogText
        //    {
        //        get { return txtDialogMessage.Text; }
        //        set { txtDialogMessage.Text = value; }
        //    }


        //    /// <summary>
        //    /// Whether to enable cancelling the process. This basically
        //    /// shows or hides the Cancel button. Defaults to false.
        //    /// </summary>
        //    public bool IsCancellingEnabled
        //    {
        //        get { return cancelButton.IsVisible; }
        //        set { cancelButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
        //    }

        //    /// <summary>
        //    /// Provides an exception that occurred during the asynchronous
        //    /// operation on the worker thread. Defaults to null, which
        //    /// indicates that no exception occurred at all.
        //    /// </summary>
        //    public Exception Error { get; private set; }


        //    /// <summary>
        //    /// The result, if assigned to the <see cref="DoWorkEventArgs.Result"/>
        //    /// property by the worker method. Defaults to null.
        //    /// </summary>
        //    public object Result { get; private set; }


        //    /// <summary>
        //    /// Inits the dialog with a given dialog text.
        //    /// </summary>
        //    public ProgressWindow(string dialogText) : this()
        //    {
        //        DialogText = dialogText;
        //    }


        //    public ProgressWindow()
        //    {
        //        InitializeComponent();

        //        //init the timer
        //        _progressTimer = new DispatcherTimer(DispatcherPriority.SystemIdle, Dispatcher);
        //        _progressTimer.Tick += OnProgressTimerTick;

        //        //init background worker
        //        _worker = new BackgroundWorker
        //        {
        //            WorkerReportsProgress = true,
        //            WorkerSupportsCancellation = true
        //        };

        //        _worker.DoWork += WorkerDoWork;
        //        _worker.ProgressChanged += WorkerProgressChanged;
        //        _worker.RunWorkerCompleted += WorkerRunWorkerCompleted;
        //    }

        //    /// <summary>
        //    /// Launches a worker thread which is intendet to perform
        //    /// work while progress is indicated.
        //    /// </summary>
        //    /// <param name="workHandler">A callback method which is
        //    /// being invoked on a background thread in order to perform
        //    /// the work to be performed.</param>
        //    public bool RunWorkerThread(DoWorkEventHandler workHandler)
        //    {
        //        return RunWorkerThread(null, workHandler);
        //    }

        //    public bool RunWorkerThread(object argument, DoWorkEventHandler workHandler)
        //    {
        //        if (_autoIncrementInterval.HasValue)
        //        {
        //            //run timer to increment progress bar
        //            _progressTimer.Interval = TimeSpan.FromMilliseconds(_autoIncrementInterval.Value);
        //            _progressTimer.Start();
        //        }

        //        _workerCallback = workHandler;
        //        _worker.RunWorkerAsync(argument);

        //        //display modal dialog (blocks caller)
        //        return ShowDialog() ?? false;
        //    }
        //    /// Worker method that gets called from a worker thread.
        //    /// Synchronously calls event listeners that may handle
        //    /// the work load.
        //    /// </summary>
        //    private void WorkerDoWork(object sender, DoWorkEventArgs e)
        //    {
        //        try
        //        {
        //            ////make sure the UI culture is properly set on the worker thread
        //            //Thread.CurrentThread.CurrentUICulture = _uiCulture;

        //            //invoke the callback method with the designated argument
        //            _workerCallback(sender, e);
        //        }
        //        catch (Exception)
        //        {
        //            //disable cancelling and rethrow the exception
        //            Dispatcher.BeginInvoke(DispatcherPriority.Normal,
        //                                   (SendOrPostCallback)delegate { cancelButton.SetValue(IsEnabledProperty, false); },
        //                                   null);

        //            throw;
        //        }
        //    }

        //    /// <summary>
        //    /// Cancels the background worker's progress.
        //    /// </summary>
        //    private void BtnCancelClick(object sender, RoutedEventArgs e)
        //    {
        //        cancelButton.IsEnabled = false;
        //        _worker.CancelAsync();
        //        Cancelled = true;
        //    }

        //    /// <summary>
        //    /// Visually indicates the progress of the background operation by
        //    /// updating the dialog's progress bar.
        //    /// </summary>
        //    private void WorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        //    {
        //        if (!Dispatcher.CheckAccess())
        //        {
        //            //run on UI thread
        //            ProgressChangedEventHandler handler = WorkerProgressChanged;
        //            Dispatcher.Invoke(DispatcherPriority.SystemIdle, handler, new[] { sender, e }, null);
        //            return;
        //        }

        //        if (e.ProgressPercentage != int.MinValue)
        //        {
        //            progressBar.Value = e.ProgressPercentage;
        //        }

        //        //lblStatus.Content = e.UserState;
        //    }

        //    /// <summary>
        //    /// Updates the user interface once an operation has been completed and
        //    /// sets the dialog's <see cref="Window.DialogResult"/> depending on the value
        //    /// of the <see cref="AsyncCompletedEventArgs.Cancelled"/> property.
        //    /// </summary>
        //    private void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //    {
        //        if (!Dispatcher.CheckAccess())
        //        {
        //            //run on UI thread
        //            RunWorkerCompletedEventHandler handler = WorkerRunWorkerCompleted;
        //            Dispatcher.Invoke(DispatcherPriority.SystemIdle, handler, new[] { sender, e }, null);
        //            return;
        //        }

        //        if (e.Error != null)
        //        {
        //            Error = e.Error;
        //        }
        //        else if (!e.Cancelled)
        //        {
        //            //assign result if there was neither exception nor cancel
        //            Result = e.Result;
        //        }

        //        //update UI in case closing the dialog takes a moment
        //        _progressTimer.Stop();
        //        progressBar.Value = progressBar.Maximum;
        //        cancelButton.IsEnabled = false;

        //        //set the dialog result, which closes the dialog
        //        DialogResult = Error == null && !e.Cancelled;
        //    }

        //    /// <summary>
        //    /// Periodically increments the value of the progress bar.
        //    /// </summary>
        //    private void OnProgressTimerTick(object sender, EventArgs e)
        //    {
        //        var threshold = 100 + _progressBarIncrement;
        //        progressBar.Value = ((progressBar.Value + _progressBarIncrement) % threshold);
        //    }

        //    /// <summary>
        //    /// Asynchronously invokes a given method on the thread
        //    /// of the dialog's dispatcher.
        //    /// </summary>
        //    /// <param name="method">The method to be invoked.</param>
        //    /// <param name="priority">The priority of the operation.</param>
        //    /// <returns>The result of the
        //    /// <see cref="Dispatcher.BeginInvoke(DispatcherPriority,Delegate)"/>
        //    /// method.</returns>
        //    public DispatcherOperation BeginInvoke(Delegate method, DispatcherPriority priority)
        //    {
        //        return Dispatcher.BeginInvoke(priority, method);
        //    }

        //    /// <summary>
        //    /// Directly updates the value of the underlying
        //    /// progress bar. This method can be invoked from a worker thread.
        //    /// </summary>
        //    /// <param name="progress"></param>
        //    /// <exception cref="ArgumentOutOfRangeException">If the
        //    /// value is not between 0 and 100.</exception>
        //    public void UpdateProgress(int progress)
        //    {
        //        if (!Dispatcher.CheckAccess())
        //        {
        //            //switch to UI thread
        //            Dispatcher.BeginInvoke(DispatcherPriority.Background,
        //                                         (SendOrPostCallback)
        //                                         delegate { UpdateProgress(progress); }, null);
        //            return;
        //        }

        //        //validate range
        //        if (progress < progressBar.Minimum || progress > progressBar.Maximum)
        //        {
        //            var msg = "Only values between {0} and {1} can be assigned to the progress bar.";
        //            msg = String.Format(msg, progressBar.Minimum, progressBar.Maximum);
        //            throw new ArgumentOutOfRangeException("progress", progress, msg);
        //        }

        //        //set the progress bar's value
        //        progressBar.SetValue(RangeBase.ValueProperty, progress);
        //    }


        //    /// <summary>
        //    /// Sets the content of the status label to a given value. This method
        //    /// can be invoked from a worker thread.
        //    /// </summary>
        //    /// <param name="status">The status to be displayed.</param>
        //    public void UpdateStatus(object status)
        //    {
        //        Dispatcher.BeginInvoke(DispatcherPriority.Background,
        //                               (SendOrPostCallback)delegate { lblStatus.SetValue(ContentProperty, status); }, null);
        //    }

        //    /// <summary>
        //    /// Synchronously invokes a given method on the thread
        //    /// of the dialog's dispatcher.
        //    /// </summary>
        //    /// <param name="method">The method to be invoked.</param>
        //    /// <param name="priority">The priority of the operation.</param>
        //    /// <returns>The result of the
        //    /// <see cref="Dispatcher.Invoke(DispatcherPriority,Delegate)"/>
        //    /// method.</returns>
        //    public object Invoke(Delegate method, DispatcherPriority priority)
        //    {
        //        return Dispatcher.Invoke(priority, method);
        //    }

    }


}
