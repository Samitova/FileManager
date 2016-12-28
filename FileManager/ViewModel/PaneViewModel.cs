using FileManager.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.ViewModel
{
    class PaneViewModel
    {
        public string[] LogicalDrivers { get; set; }
        public string CurrentDriver { get; set; }

        public MyDirInfo CurrentItem { get; set; }

        private MyDirInfo _currentDirectory;

        private IList<MyDirInfo> _currentItems;

        public MyDirInfo CurrentDirectory
        {
            get { return _currentDirectory; }
            set
            {
                _currentDirectory = value;
                RefreshCurrentItems();
                //OnPropertyChanged("CurrentDirectory");
            }
        }

        /// <summary>
        /// Children of the current directory 
        /// </summary>
        public IList<MyDirInfo> CurrentItems
        {
            get
            {
                if (_currentItems == null)
                {
                    _currentItems = new List<MyDirInfo>();
                }
                return _currentItems;
            }
            set
            {
                _currentItems = value;
                //OnPropertyChanged("CurrentItems");
            }
        }

        public PaneViewModel()
        {           
            CurrentDirectory = new MyDirInfo(new DirectoryInfo(@"c:\"));
        }

        public PaneViewModel(string driver)
        {
            CurrentDirectory = new MyDirInfo(new DirectoryInfo(driver));
        }

        /// <summary>
        /// Get the children of current directory and stores them in the CurrentItems Observable collection
        /// </summary>
        protected void RefreshCurrentItems()
        {
            IList<MyDirInfo> childrenDirList = new List<MyDirInfo>();
            IList<MyDirInfo> childrenFileList = new List<MyDirInfo>();

            childrenDirList = FileSystemExplorerService.GetChildDirectories(CurrentDirectory.Path).Select(dir => new MyDirInfo(dir)).ToList();

            childrenFileList = FileSystemExplorerService.GetChildFiles(CurrentDirectory.Path).Select(dir => new MyDirInfo(dir)).ToList();

            CurrentItems = childrenDirList.Concat(childrenFileList).ToList(); 
        }

        /// <summary>
        /// processes the current object. If this is a file then open it or if it is a directory then return its subdirectories
        /// </summary>
        public void OpenCurrentItem()
        {           
            if ((MyDirectoryType)CurrentItem.Type  == MyDirectoryType.File)
            {
                System.Diagnostics.Process.Start(CurrentItem.Path);
            }
            else
            {
                
            }
        }

    }
}
