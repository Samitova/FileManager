using FileManager.Model;
using FileManager.View;
using FileManager.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Collections.Specialized;
using System.Windows;

namespace FileManager.ViewModel
{
    class SearchViewModel : ViewModelBase
    {       
        public ObservableCollection<string> Files { get; set; }

        public PaneViewModel PaneVM { get; set; }
        public AsyncRelayCommand SearchCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public RelayCommand StopCommand { get; set; }
        public SearchView CurrentWindow { get; set; }

        private string _searchingPath;
        private string _searchingPattern;
        private string _currentSearchingDirectory;
        private string _selectedFoundFile;

        public string SearchingPath
        {
            get { return _searchingPath; }
            set
            {
                _searchingPath = value;
                OnPropertyChanged("SearchingPath");
            }
        }

        public string SearchingPattern
        {
            get { return _searchingPattern; }
            set
            {
                _searchingPattern = value;
                OnPropertyChanged("SearchingPattern");
            }
        }

        public string CurrentSearchingDirectory
        {
            get { return _currentSearchingDirectory; }
            set
            {
                _currentSearchingDirectory = value;
                OnPropertyChanged("CurrentSearchingDirectory");
            }
        }

        public string SelectedFoundFile
        {
            get { return _selectedFoundFile; }
            set
            {
                _selectedFoundFile = value;
                OnPropertyChanged("SelectedFoundFile");
            }
        }      

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="paneViewModel"></param>
        public SearchViewModel(PaneViewModel paneVM)
        {
            PaneVM = paneVM;
            SearchCommand = new AsyncRelayCommand(SearchFiles);
            CloseCommand = new RelayCommand(Close);
            StopCommand = new RelayCommand(StopSearching);

            Files = new ObservableCollection<string>();
            Files.CollectionChanged += Files_CollectionChanged;
            
            FileSystemProvider.FoundItem += RefreshFoundFiles;
            FileSystemProvider.ChangeDirectory += (o, ea) => CurrentSearchingDirectory = FileSystemProvider.CurrentSearchDir;
            SearchingPath = @"c:\";       
        }

        /// <summary>
        /// Invokes when observable collection is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Files_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add: 
                    string newFile = e.NewItems[0] as string;
                    break;
                case NotifyCollectionChangedAction.Remove: 
                    string oldFile = e.OldItems[0] as string;
                    break;
                case NotifyCollectionChangedAction.Replace: 
                    string replacedUser = e.OldItems[0] as string;
                    string replacingUser = e.NewItems[0] as string;
                    break;
            }
        }

        /// <summary>
        /// Init Searching files
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="cancel"></param>
        private void SearchFiles(Action<int> progress, Func<bool> cancel)
        {
            if (SearchingPattern != null)
            {
                Application.Current.Dispatcher.Invoke(
                            () =>
                            {
                                Files.Clear();
                            });
               
                CurrentSearchingDirectory = SearchingPath;
                FileSystemProvider.SearchFiles(SearchingPattern, SearchingPath, progress, cancel);
            }
        }

        /// <summary>
        /// Stop searching if command executes
        /// </summary>
        private void StopSearching()
        {
            if (SearchCommand.IsExecuting)
            {
                SearchCommand.IsCancellationRequested = true;
            }
        }

        /// <summary>
        /// Close searching window
        /// </summary>
        private void Close()
        {
            StopSearching();
            CurrentWindow.Close();
        }

        private void RefreshFoundFiles(object sender, EventArgs e)
        {            
            Application.Current.Dispatcher.Invoke(
                            () =>
                            {
                                Files.Add(FileSystemProvider.CurrentFoundFile);
                            });
        }
          
        /// <summary>
        /// On double click go to selected file directory
        /// </summary>
        internal void GoToSelectedFile()
        {
            if (SelectedFoundFile != null)
            {
                MyDirInfo dir = new MyDirInfo(FileSystemProvider.GetFile(SelectedFoundFile).Directory);
                PaneVM.CurrentItem = dir;
                CurrentWindow.Close();
            }
        }

    }
}
