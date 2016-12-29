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

        private SystemFileItem _currentDriver;
        private IList<SystemFileItem> _drivers;
        private SystemFileItem _currentItem;
        private IList<SystemFileItem> _visibleItems;

        public SystemFileItem CurrentItem
        {
            get { return _currentItem; }
            set
            {
                _currentItem = value;
                RefreshVisibleItems();
                OnPropertyChanged("CurrentItem");
            }
        }

        public SystemFileItem CurrentDriver
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

        public IList<SystemFileItem> LogicalDrivers
        {
            get
            {
                if (_drivers == null)
                {
                    _drivers = new List<SystemFileItem>();
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
        public IList<SystemFileItem> VisibleItems
        {
            get
            {
                if (_visibleItems == null)
                {
                    _visibleItems = new List<SystemFileItem>();
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
            CurrentDriver = new MyDriveInfo(FileSystemProvider.(@"c:\"));
            CurrentItem = CurrentDriver;
        }

        /// <summary>
        /// Get the children of current directory and stores them in the CurrentItems Observable collection
        /// </summary>
        protected void RefreshDrivers()
        { 
            LogicalDrivers = FileSystemProvider.GetLocalDrivers().Select(dir => new MyDriveInfo(dir)).ToList<SystemFileItem>();               
        }

        /// <summary>
        /// Get the children of current directory and stores them in the VisibleItems observable collection
        /// </summary>
        protected void RefreshVisibleItems()
        {

            IList<SystemFileItem> childrenDirList = new List<SystemFileItem>();           

            if (CurrentItem is MyDirInfo)
            {
                CurrentItem.Name = "[...]";
                childrenDirList.Add(CurrentItem);
            }            
           
            childrenDirList = childrenDirList.Concat(FileSystemProvider.GetChildrenDirectories(CurrentItem.Path).Select(dir => new MyDirInfo(dir)).ToList<SystemFileItem>()).ToList();
            VisibleItems = childrenDirList.Concat(FileSystemProvider.GetChildrenFiles(CurrentItem.Path).Select(dir => new MyFileInfo(dir)).ToList<SystemFileItem>()).ToList();

        }

        /// <summary>
        /// Execure the current object. If it is the file then open; if it is the directory then return its subdirectories
        /// </summary>
        public void OpenCurrentItem(SystemFileItem selectedItem)
        {
            if (selectedItem is MyFileInfo)
            {
                System.Diagnostics.Process.Start(selectedItem.Path);
            }
           
                if (CurrentItem == selectedItem && !selectedItem.Root.EndsWith(@"\"))
                {
                    CurrentItem = new MyDirInfo(new DirectoryInfo(selectedItem.Root));
                }
                else if (CurrentItem == selectedItem)
                {
                    CurrentItem = new MyDriveInfo(new DriveInfo(selectedItem.Root));
                }
                else
                {
                    CurrentItem = selectedItem;
                }            
        }

    }
}
