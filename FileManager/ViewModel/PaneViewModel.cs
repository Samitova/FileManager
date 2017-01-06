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

        private SystemFileItem _currentDrive;
        private IList<SystemFileItem> _drives;
        private SystemFileItem _currentItem;
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
                _currentItem = value;
                RefreshVisibleItems();
                OnPropertyChanged("CurrentItem");
            }
        }

        public SystemFileItem CurrentDrive
        {
            get { return _currentDrive; }
            set
            {
                _currentDrive = value;
                CurrentItem = _currentDrive;
                //RefreshDrivers();
                OnPropertyChanged("CurrentDrive");
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
            CurrentDrive = new MyDriveInfo(FileSystemProvider.GetDrive(@"c:\"));
            CurrentItem = CurrentDrive;          

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
        public void Move(AsynchronousCommand command)
        {
            int filesCount = GetFilesCount(SelectedItems);
            string message = $"Do you want to move {filesCount} files to {DirectoryToCopy}?";
           
            MessageBoxResult resultDeleteConformation = MessageBox.Show(message, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultDeleteConformation == MessageBoxResult.Yes)
            {
                foreach (SystemFileItem item in SelectedItems)
                {
                    if (command.CancelIfRequested())
                        return;
                    command.ReportProgress(() => item.Move(DirectoryToCopy));
                    command.ProgressStatus += 100 / filesCount;
                    System.Threading.Thread.Sleep(2000);
                }
            }            
        }

        /// <summary>
        /// Delete selected items
        /// </summary>
        /// <param name="selectedItems">selectedItems</param>
        public void Delete(AsynchronousCommand command)
        {
            int filesCount = GetFilesCount(SelectedItems);
            string message = $"Do you want to delete {filesCount} files?";

            MessageBoxResult resultDeleteConformation = MessageBox.Show(message, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resultDeleteConformation == MessageBoxResult.Yes)
            {
                foreach (SystemFileItem item in SelectedItems)
                {
                    if (command.CancelIfRequested())
                        return;
                    command.ReportProgress(() => item.Delete());
                    command.ProgressStatus += 100 / filesCount;
                    System.Threading.Thread.Sleep(2000);
                }
            }            
        }

        /// <summary>
        /// Copy selected items
        /// </summary>
        /// <param name="selectedItems">selectedItems</param>
        public void Copy(AsynchronousCommand command)
        {
            int filesCount = GetFilesCount(SelectedItems);
            string message = $"Do you want to copy {filesCount} files to {DirectoryToCopy}?";

            MessageBoxResult resultDeleteConformation = MessageBox.Show(message, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultDeleteConformation == MessageBoxResult.Yes)
            {
               
                foreach (SystemFileItem item in SelectedItems)
                {
                    if (command.CancelIfRequested())
                        return;
                    command.ReportProgress(() => item.Copy(DirectoryToCopy));
                    command.ProgressStatus += 100/filesCount;
                    System.Threading.Thread.Sleep(2000);
                }
            }
        }

        /// <summary>
        /// Get totla count of files
        /// </summary>
        /// <param name="selectedItems">selected files and directories</param>
        /// <returns></returns>
        private int GetFilesCount(IList<SystemFileItem> selectedItems)
        {
            int filesCount = 0;

            foreach (SystemFileItem item in selectedItems)
            {
                filesCount+=item.TotalSubFilesCount;
            }

            return filesCount;
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
        }
      
    }
}
