using FileManager.Model;
using FileManager.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace FileManager.ViewModel
{

    class PaneViewModel : ViewModelBase
    {
        public event EventHandler NeedsUpdateSource;
      
        private SystemFileItem _currentDrive;
        private SystemFileItem _currentItem;
        private IList<SystemFileItem> _drives;
        private IList<SystemFileItem> _visibleItems;
        private IList<SystemFileItem> _selectedItems;        

        public string DirectoryToCopy { get; set; }

        public SystemFileItem SelectedItem { get; set; }

        public IList<SystemFileItem> SelectedItems
        {
            get
            {
                if (_selectedItems == null)
                {
                    _selectedItems = new List<SystemFileItem>();
                }
                return _selectedItems;
            }
            set
            {
                _selectedItems = value;
                OnPropertyChanged("SelectedItems");
            }
        }      

        public SystemFileItem CurrentItem
        {
            get { return _currentItem; }
            set
            {
                var temp = _currentItem;

                _currentItem = value; 
                if(!value.Equals(temp))               
                    RefreshVisibleItems();
                OnPropertyChanged("CurrentItem");
            }
        }

        public SystemFileItem CurrentDrive
        {
            get { return _currentDrive; }
            set
            {
                if (value != null)
                {
                    _currentDrive = value;
                    if (_currentDrive != null)
                        CurrentItem = _currentDrive;                   
                    OnPropertyChanged("CurrentDrive");
                }                   
            }
        }

        public IList<SystemFileItem> LogicalDrives
        {
            get
            {
                if (_drives == null)
                {
                    _drives = new List<SystemFileItem>();
                }
                return _drives;
            }
            set
            {
                _drives = value;
                OnPropertyChanged("LogicalDrives");               
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

        /// <summary>
        /// ctor
        /// </summary>
        public PaneViewModel()
        {
            RefreshDrivers();
            CurrentDrive = new MyDriveInfo(FileSystemProvider.GetDrive(@"C:\"));            
        }

        /// <summary>
        /// Get the children of current directory and stores them in the CurrentItems Observable collection
        /// </summary>
        public void RefreshDrivers()
        {
            LogicalDrives = FileSystemProvider.GetLocalDrives().Select(dir => new MyDriveInfo(dir)).ToList<SystemFileItem>(); 
        }

        /// <summary>
        /// Get the children of current directory and stores them in the VisibleItems observable collection
        /// </summary>
        public void RefreshVisibleItems()
        {
            VisibleItems = CurrentItem.GetChildren();
        }

        /// <summary>
        /// Execure the current object. If it is the file then open; if it is the directory then return its subdirectories
        /// </summary>
        public void OpenCurrentItem()
        {
            if (SelectedItem is MyFileInfo)
            {
                SelectedItem.Execute();
            }
            else if (CurrentItem == SelectedItem && !SelectedItem.Parent.EndsWith(@"\"))
            {
                CurrentItem = new MyDirInfo(new DirectoryInfo(SelectedItem.Parent));
            }
            else if (CurrentItem == SelectedItem)
            {
                CurrentItem = new MyDriveInfo(new DriveInfo(SelectedItem.Parent));
            }
            else
            {
                CurrentItem = SelectedItem;
            }
        }

        /// <summary>
        /// Move selected items
        /// </summary>
        /// <param name="selectedItems">selectedItems</param>
        public void Move(Action<int> progress, Func<bool> cancel)
        {            
            int counter = 0;

            foreach (SystemFileItem item in SelectedItems)
            {
                if (cancel())
                {
                    OnNeedsUpdateSource(new EventArgs());
                    return;
                }
                item.Move(DirectoryToCopy);
                progress(counter += 100 / SelectedItems.Count);
                System.Threading.Thread.Sleep(1000);
            }
            OnNeedsUpdateSource(new EventArgs());
        }

        /// <summary>
        /// Delete selected items
        /// </summary>
        /// <param name="selectedItems">selectedItems</param>
        public void Delete(Action<int> progress, Func<bool> cancel)
        {
            int counter = 0;

            foreach (SystemFileItem item in SelectedItems)
            {
                if (cancel())
                {
                    OnNeedsUpdateSource(new EventArgs());
                    return;
                }
                item.Delete();
                progress(counter += 100 / SelectedItems.Count);
                System.Threading.Thread.Sleep(2000);
            }
            OnNeedsUpdateSource(new EventArgs());
        }

        /// <summary>
        /// Copy selected items
        /// </summary>
        /// <param name="selectedItems">selectedItems</param>
        internal void Copy(Action<int> progress, Func<bool> cancel)
        {           
            int counter = 0;

            foreach (SystemFileItem item in SelectedItems)
            {
                if (cancel())
                {
                    OnNeedsUpdateSource(new EventArgs());
                    return;
                }
                item.Copy(DirectoryToCopy);
                progress(counter += 100 / SelectedItems.Count);
                System.Threading.Thread.Sleep(2000);
            }
            OnNeedsUpdateSource(new EventArgs());
        }      

        /// <summary>
        /// Get details about system item
        /// </summary>
        public void GetDetails()
        {
            SelectedItem.GetDetails();
        }

        /// <summary>
        /// Create directory
        /// </summary>
        /// <param name="dirName">directory name</param>
        public void Create(string dirName)
        {
            if (CurrentItem != null)
            {
                CurrentItem.Create(dirName);
            }
            OnNeedsUpdateSource(new EventArgs());
        }

        /// <summary>
        /// Rename selected system item
        /// </summary>
        /// <param name="newName"></param>
        public void Rename(string newName)
        {
            if (SelectedItem != null)
            {
                SelectedItem.Rename(newName);
            }
            OnNeedsUpdateSource(new EventArgs());
        }

        public void OnNeedsUpdateSource(EventArgs e)
        {
            NeedsUpdateSource?.Invoke(this, e);           
        }      
    }
}
