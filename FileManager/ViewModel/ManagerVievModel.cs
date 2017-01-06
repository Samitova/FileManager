using FileManager.View;
using FileManager.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FileManager.ViewModel
{
    internal class ManagerVievModel : ViewModelBase
    {
        private PaneViewModel _currentPaneViewModel;
       
        public PaneViewModel LeftPaneViewModel { get; set; }
        public PaneViewModel RightPaneViewModel { get; set; }
        public PaneViewModel CurrentPaneViewModel
        {
            get { return _currentPaneViewModel; }
            set
            {
                _currentPaneViewModel = value;
                OnPropertyChanged("CurrentPaneViewModel");
            }
        }

        public Command CreateCommand { get; set; }
        public Command DetailsCommand { get; set; }

        public Command StartCopyCommand { get; set; }
        public Command StartMoveCommand { get; set; }
        public Command StartDeleteCommand { get; set; }

        public AsynchronousCommand AsyncCopyCommand { get; set; }
        public AsynchronousCommand AsyncMoveCommand { get; set; }
        public AsynchronousCommand AsyncDeleteCommand { get; set; }


        /// <summary>
        /// ctor
        /// </summary>
        public ManagerVievModel()
        {
            LeftPaneViewModel = new PaneViewModel();
            RightPaneViewModel = new PaneViewModel();

            CurrentPaneViewModel = LeftPaneViewModel;

            StartCopyCommand = new Command(StartCopy);
            DetailsCommand = new Command(GetDetails);
            CreateCommand = new Command(CreateDirectory);
            StartDeleteCommand = new Command(StartDelete);
            StartMoveCommand = new Command(StartMove);

            AsyncCopyCommand = new AsynchronousCommand(AsyncCopy);
            AsyncCopyCommand.Executed += RefreshView;
            AsyncMoveCommand = new AsynchronousCommand(AsyncMove);
            AsyncMoveCommand.Executed += RefreshView;
            AsyncDeleteCommand = new AsynchronousCommand(AsyncDelete);
            AsyncDeleteCommand.Executed += RefreshView;

        }

        private void AsyncDelete()
        {
            CurrentPaneViewModel.Delete(AsyncDeleteCommand);
        }

        private void AsyncMove()
        {
            CurrentPaneViewModel.Move(AsyncMoveCommand);
        }

        private void StartMove()
        {
            AsyncMoveCommand.DoExecute(null);
            if(AsyncMoveCommand.ProgressStatus > 0)
                InitProgressWindow(AsyncCopyCommand);
            
        }

        private void StartDelete()
        {
            AsyncDeleteCommand.DoExecute(null);
            if (AsyncDeleteCommand.ProgressStatus > 0)
                InitProgressWindow(AsyncCopyCommand);           
        }

        public void StartCopy()
        {
            AsyncCopyCommand.DoExecute(null);
            if (AsyncDeleteCommand.ProgressStatus > 0)
                InitProgressWindow(AsyncCopyCommand);
           
        }

        internal void GetDetails()
        {
            CurrentPaneViewModel.GetDetails();
        }

        internal void CreateDirectory()
        {
            string name = GetCreatingFileName();
            if (name != null)
            {
                CurrentPaneViewModel.Create(name);
                RefreshView();
            }            
        }

        internal void AsyncCopy()
        {
            CurrentPaneViewModel.Copy(AsyncCopyCommand);
        }

        private void RefreshView(object sender, CommandEventArgs args)
        {
            LeftPaneViewModel.RefreshVisibleItems();
            RightPaneViewModel.RefreshVisibleItems();
        }

        private void RefreshView()
        {
            LeftPaneViewModel.RefreshVisibleItems();
            RightPaneViewModel.RefreshVisibleItems();
        }

        /// <summary>
        /// Init progrees window 
        /// </summary>        
        private void InitProgressWindow(AsynchronousCommand command)
        {
            ProgressWindow window = new ProgressWindow() { DataContext = command };
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Show();
        }


        /// <summary>
        /// Get creating directory name
        /// </summary>
        /// <returns></returns>
        private string GetCreatingFileName()
        {
            CreateDirectory createDirWindow = new CreateDirectory();            
            createDirWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            createDirWindow.ShowDialog();
            return createDirWindow.DirName;
        }
    }
}
