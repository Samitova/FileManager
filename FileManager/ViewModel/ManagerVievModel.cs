using FileManager.View;
using FileManager.ViewModel.Commands;
using System;
using System.Windows;


namespace FileManager.ViewModel
{
    internal class ManagerVievModel : ViewModelBase
    {
        private PaneViewModel _currentPaneViewModel;        

        public PaneViewModel LeftPaneViewModel { get; set; }
        public PaneViewModel RightPaneViewModel { get; set; }

        public RelayCommand CreateCommand { get; set; }
        public RelayCommand DetailsCommand { get; set; }

        public RelayCommand StartSearchCommand { get; set; }
        public RelayCommand StartCopyCommand { get; set; }
        public RelayCommand StartMoveCommand { get; set; }
        public RelayCommand StartDeleteCommand { get; set; }

        public AsyncRelayCommand AsyncCopyCommand { get; set; }
        public AsyncRelayCommand AsyncMoveCommand { get; set; }
        public AsyncRelayCommand AsyncDeleteCommand { get; set; }

        public PaneViewModel CurrentPaneViewModel
        {
            get { return _currentPaneViewModel; }
            set
            {
                _currentPaneViewModel = value;
                OnPropertyChanged("CurrentPaneViewModel");
            }
        }
      
        /// <summary>
        /// ctor
        /// </summary>
        public ManagerVievModel()
        {
            LeftPaneViewModel = new PaneViewModel();
            RightPaneViewModel = new PaneViewModel();

            CurrentPaneViewModel = LeftPaneViewModel;

            InitCommands();
        }

        /// <summary>
        /// Init manager commands
        /// </summary>
        private void InitCommands()
        {
            DetailsCommand = new RelayCommand(GetDetails);
            CreateCommand = new RelayCommand(CreateDirectory);
            StartSearchCommand = new RelayCommand(StartSearchFiles);
            StartDeleteCommand = new RelayCommand(StartDelete);
            StartMoveCommand = new RelayCommand(StartMove);
            StartCopyCommand = new RelayCommand(StartCopy);

            AsyncCopyCommand = new AsyncRelayCommand(AsyncCopy);
            AsyncMoveCommand = new AsyncRelayCommand(AsyncMove);
            AsyncDeleteCommand = new AsyncRelayCommand(AsyncDelete);       

        }

        /// <summary>
        /// Init move process
        /// </summary>
        private void StartMove()
        {
            string message = $"Do you wand to move {CurrentPaneViewModel.GetFilesCount()} files?";
            InitProgressWindow(message, AsyncMoveCommand);
        }

        /// <summary>
        /// Init delete process
        /// </summary>
        private void StartDelete()
        {
            string message = $"Do you wand to delete {CurrentPaneViewModel.GetFilesCount()} files?";
            InitProgressWindow(message, AsyncDeleteCommand);
        }

        /// <summary>
        /// Init copy process
        /// </summary>
        public void StartCopy()
        {
            string message = $"Do you wand to copy {CurrentPaneViewModel.GetFilesCount()} files?";
            InitProgressWindow(message, AsyncCopyCommand);            
        }

        /// <summary>
        /// Async delete function
        /// </summary>
        /// <param name="progress">Action to get delete progress</param>
        /// <param name="cancel">Func to cancel delete </param>
        private void AsyncDelete(Action<int> progress, Func<bool> cancel)
        {
            CurrentPaneViewModel.Delete(progress, cancel);
        }

        /// <summary>
        /// Async move function
        /// </summary>
        /// <param name="progress">Action to get move progress</param>
        /// <param name="cancel">Func to cancel move </param>
        private void AsyncMove(Action<int> progress, Func<bool> cancel)
        {
            CurrentPaneViewModel.Move(progress, cancel);
        }

        /// <summary>
        /// Async copy function
        /// </summary>
        /// <param name="progress">Action to get copy progress</param>
        /// <param name="cancel">Func to cancel copy </param>
        internal void AsyncCopy(Action<int> progress, Func<bool> cancel)
        {
            CurrentPaneViewModel.Copy(progress, cancel);
        }

        /// <summary>
        /// Get details
        /// </summary>
        internal void GetDetails()
        {
            CurrentPaneViewModel.GetDetails();
        }

        /// <summary>
        /// Create directory
        /// </summary>
        internal void CreateDirectory()
        {
            string name = GetCreatingFileName();
            if (name != null)
            {
                CurrentPaneViewModel.Create(name);                
            }
        }

        /// <summary>
        /// Invoke progrees window 
        /// </summary>        
        private void InitProgressWindow(string massage, AsyncRelayCommand asyncCommand)
        {
            ProgressViewModel progressModel = new ProgressViewModel(massage, asyncCommand);
            ProgressWindow window = new ProgressWindow() { DataContext = progressModel };
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            progressModel.CurrentWindow = window;
            window.Show();
        }

        /// <summary>
        /// Invoke window to get name for creating directory 
        /// </summary>
        /// <returns></returns>
        private string GetCreatingFileName()
        {
            CreateDirectory createDirWindow = new CreateDirectory();
            createDirWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            createDirWindow.ShowDialog();
            return createDirWindow.DirName;
        }

        /// <summary>
        /// Invoke search window
        /// </summary>
        private void StartSearchFiles()
        {
            SearchViewModel searchViewModel = new SearchViewModel(LeftPaneViewModel);
            SearchView searchWindow = new SearchView() { DataContext = searchViewModel };
            searchWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            searchViewModel.CurrentWindow = searchWindow;
            searchWindow.Show();
        }
               
    }
}
