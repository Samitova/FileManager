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
        private IList<MyDirInfo> _visibleItems;

        public MyDirInfo CurrentItem
        {
            get { return _currentItem; }
            set
            {
                _currentItem = value;
                RefreshVisibleItems();
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
        public IList<MyDirInfo> VisibleItems
        {
            get
            {
                if (_visibleItems == null)
                {
                    _visibleItems = new List<MyDirInfo>();
                }
                return _visibleItems;
            }
            set
            {
                _visibleItems = value;
                OnPropertyChanged("VisibleItems");
            }
        }

        public PaneViewModel()
        {
            RefreshDrivers();
            CurrentDriver = new MyDirInfo(new DriveInfo(@"c:\"));
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
        /// Get the children of current directory and stores them in the VisibleItems observable collection
        /// </summary>
        protected void RefreshVisibleItems()
        {

            IList<MyDirInfo> childrenDirList = new List<MyDirInfo>();
            IList<MyDirInfo> childrenFileList = new List<MyDirInfo>();

            if ((SystemType)CurrentItem.Type != SystemType.Driver)
            {
                CurrentItem.Name = "[...]";
                childrenDirList.Add(CurrentItem);
            }                

            childrenFileList = FileSystemProvider.GetChildFiles(CurrentItem.Path).Select(dir => new MyDirInfo(dir)).ToList();
            childrenDirList = childrenDirList.Concat(FileSystemProvider.GetChildDirectories(CurrentItem.Path).Select(dir => new MyDirInfo(dir)).ToList()).ToList();
            VisibleItems = childrenDirList.Concat(childrenFileList).ToList();

        }

        /// <summary>
        /// Execure the current object. If it is the file then open; if it is the directory then return its subdirectories
        /// </summary>
        public void OpenCurrentItem(MyDirInfo selectedItem)
        {
            if ((SystemType)selectedItem.Type == SystemType.File)
            {
                System.Diagnostics.Process.Start(selectedItem.Path);
            }
            else
            {
                if (CurrentItem == selectedItem && !selectedItem.Root.EndsWith(@"\"))
                {
                    CurrentItem = new MyDirInfo(new DirectoryInfo(selectedItem.Root));
                }
                else if (CurrentItem == selectedItem)
                {
                    CurrentItem = new MyDirInfo(new DriveInfo(selectedItem.Root));
                }
                else
                {
                    CurrentItem = selectedItem;
                }
            }
        }

    }
}
