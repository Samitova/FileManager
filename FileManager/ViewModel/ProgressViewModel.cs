using FileManager.View;
using FileManager.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileManager.ViewModel
{
    class ProgressViewModel : ViewModelBase
    {       
        public string Message { get; set; }

        public AsyncRelayCommand AsyncCommand { get; set; }

        public RelayCommand CloseCommand { get; set; }

        public ProgressWindow CurrentWindow { get; set; }

        public ProgressViewModel()
        { }

        public ProgressViewModel(string message, AsyncRelayCommand asyncCommand)
        {
            Message = message;
            AsyncCommand = asyncCommand;
            CloseCommand = new RelayCommand(Close);
            AsyncCommand.Executed += CloseOnExecuted;
        }

        /// <summary>
        /// Close on user demand
        /// </summary>
        private void Close()
        {
            if (AsyncCommand.IsExecuting)
            {
                AsyncCommand.IsCancellationRequested = true;
            }

            CurrentWindow.Close();
        }

        /// <summary>
        /// Close on executed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void CloseOnExecuted(object sender, RunWorkerCompletedEventArgs args)
        {
            CurrentWindow.Close();            
        }
    }
}
