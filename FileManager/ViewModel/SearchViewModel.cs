using FileManager.Model;
using FileManager.View;
using FileManager.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.ViewModel
{
    class SearchViewModel: ViewModelBase
    {
        public AsyncRelayCommand SearchCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public RelayCommand StopCommand { get; set; }
        public SearchView CurrentWindow { get; set; }

        private string _searchingPath;
        private string _searchingPattern;
        private string _currentSearchingDirectory;
        private List<string> _foundFiles;
       
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

        public List<string> FoundFiles
        {
            get
            {
                if (_foundFiles == null)
                {
                    _foundFiles = new List<string>();
                }
                return _foundFiles;
            }
            set
            {
                _foundFiles = value;
                OnPropertyChanged("FoundFiles");
            }
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="paneViewModel"></param>
        public SearchViewModel()
        {
            SearchCommand = new AsyncRelayCommand(SearchFiles);
            CloseCommand = new RelayCommand(Close);
            StopCommand = new RelayCommand(StopSearching);
            FileSystemProvider.FoundItem += (o, ea) => FoundFiles = FileSystemProvider.FoundItems;
            FileSystemProvider.ChangeDirectory += (o, ea) => CurrentSearchingDirectory = FileSystemProvider.CurrentSearchDir;
            SearchingPath = @"c:\";             
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
        /// Init Searching files
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="cancel"></param>
        private void SearchFiles(Action<int> progress, Func<bool> cancel)
        {
            if (SearchingPattern != null)
            {
                FoundFiles.Clear();
                CurrentSearchingDirectory = SearchingPath;
                FileSystemProvider.SearchFiles(SearchingPattern, SearchingPath, progress, cancel);
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
    }
}
