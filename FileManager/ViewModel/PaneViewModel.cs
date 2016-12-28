using FileManager.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.ViewModel
{
    class PaneViewModel : ViewModelBase
    {
        private MyDirInfo _currentDriver;
        private IList<MyDirInfo> _drivers;
        private MyDirInfo _currentItem;
        private IList<MyDirInfo> _currentItems;

        public MyDirInfo CurrentItem
        {
            get { return _currentItem; }
            set
            {
                _currentItem = value;
                RefreshCurrentItems();
                OnPropertyChanged("CurrentItem");
            }
        }

        public MyDirInfo CurrentDriver
        {
            get { return _currentDriver; }
            set
            {
                _currentDriver = value;
                CurrentItem = _currentDriver;
                //RefreshDrivers();
                OnPropertyChanged("CurrentDriver");
            }
        }

        public IList<MyDirInfo> LogicalDrivers
        {
            get
            {
                if (_drivers == null)
                {
                    _drivers = new List<MyDirInfo>();
                }
                return _drivers;
            }
            set
            {
                _drivers = value;
                OnPropertyChanged("LogicalDrivers");
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
                OnPropertyChanged("CurrentItems");
            }
        }

        public PaneViewModel()
        {
            RefreshDrivers();
            CurrentDriver = new MyDirInfo(new DriveInfo(@"c:\"));
            CurrentItem = CurrentDriver;
        }

        public PaneViewModel(string driver)
        {
            CurrentDriver = new MyDirInfo(new DriveInfo(driver));
            CurrentItem = CurrentDriver;
        }

        /// <summary>
        /// Get the children of current directory and stores them in the CurrentItems Observable collection
        /// </summary>
        protected void RefreshDrivers()
        {
            LogicalDrivers = FileSystemProvider.GetLocalDrivers().Select(dir => new MyDirInfo(dir)).ToList();
        }

        /// <summary>
        /// Get the children of current directory and stores them in the CurrentItems Observable collection
        /// </summary>
        protected void RefreshCurrentItems()
        {           
            
            IList<MyDirInfo> childrenDirList = new List<MyDirInfo>();
            IList<MyDirInfo> childrenFileList = new List<MyDirInfo>();

            childrenDirList = FileSystemProvider.GetChildDirectories(CurrentItem.Path).Select(dir => new MyDirInfo(dir)).ToList();

            childrenFileList = FileSystemProvider.GetChildFiles(CurrentItem.Path).Select(dir => new MyDirInfo(dir)).ToList();
            if ((MyDirectoryType)CurrentItem.Type == MyDirectoryType.Driver)
            {
                CurrentItems = childrenDirList.Concat(childrenFileList).ToList();
            }
            else
            {
                childrenDirList.Add(new MyDirInfo(new DirectoryInfo(CurrentItem.Root)));
                childrenDirList
            }

            
        }

        /// <summary>
        /// Execure the current object. If it is the file then open; if it is the directory then return its subdirectories
        /// </summary>
        public void OpenCurrentItem(MyDirInfo selectedItem)
        {
            if ((MyDirectoryType)selectedItem.Type == MyDirectoryType.File)
            {
                System.Diagnostics.Process.Start(selectedItem.Path);
            }
            else
            {
                CurrentItem = selectedItem;
            }
        }

    }
}
