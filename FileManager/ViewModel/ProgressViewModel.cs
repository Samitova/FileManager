using FileManager.View;
using FileManager.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileManager.ViewModel
{
    class ProgressViewModel : ViewModelBase
    {       
        public string Message { get; set; }

        public AsynchronousCommand AsyncCommand { get; set; }

        public Command CloseCommand { get; set; }

        public ProgressWindow CurrentWindow { get; set; }

        public ProgressViewModel()
        { }

        public ProgressViewModel(string message, AsynchronousCommand asyncCommand)
        {
            Message = message;
            AsyncCommand = asyncCommand;
            CloseCommand = new Command(Close);
            AsyncCommand.Executed += CloseOnExecuted;
        }

        /// <summary>
        /// Close on user demand
        /// </summary>
        private void Close()
        {
            if (AsyncCommand.IsExecuting)
            {
                AsyncCommand.CancelCommand.DoExecute(null);
            }

            CurrentWindow.Close();
        }

        /// <summary>
        /// Close on executed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void CloseOnExecuted(object sender, CommandEventArgs args)
        {
            CurrentWindow.Close();
        }
    }
}
