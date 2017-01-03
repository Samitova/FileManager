using FileManager.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileManager.ViewModel
{
    internal class PaneViewModel : ViewModelBase
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

        /// <summary>
        /// ctor
        /// </summary>
        public PaneViewModel()
        {
            RefreshDrivers();
            CurrentDrive = new MyDriveInfo(FileSystemProvider.GetDrive(@"c:\"));
            CurrentItem = CurrentDrive;

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
        /// Get the children of current directory and stores them in the CurrentItems Observable collection
        /// </summary>
        protected void RefreshDrivers()
        {
            LogicalDrives = FileSystemProvider.GetLocalDrives().Select(dir => new MyDriveInfo(dir)).ToList<SystemFileItem>();
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

        public void Move(IList<SystemFileItem> selectedItems)
        {
            string[] messageParams = new string[] { "Do you want to move ", "to", DirectoryToCopy, "?" };
            MessageBoxResult resultDeleteConformation = MessageBox.Show(BuildMassege(selectedItems, messageParams), "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultDeleteConformation == MessageBoxResult.Yes)
            {
                foreach (SystemFileItem item in selectedItems)
                {
                    item.Move(DirectoryToCopy);
                }
            }
            RefreshVisibleItems();
        }

        /// <summary>
        /// Delete selected items
        /// </summary>
        /// <param name="selectedItems">selectedItems</param>
        public void Delete(IList<SystemFileItem> selectedItems)
        {
            string[] messageParams = new string[] { "Do you want to delete ", "?" };

            MessageBoxResult resultDeleteConformation = MessageBox.Show(BuildMassege(selectedItems, messageParams), "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resultDeleteConformation == MessageBoxResult.Yes)
            {
                foreach (SystemFileItem item in selectedItems)
                {
                    item.Delete();
                }
            }
            RefreshVisibleItems();
        }

        public void Copy(IList<SystemFileItem> selectedItems)
        {
            string[] messageParams = new string[] { "Do you want to copy ", "to", DirectoryToCopy, "?" };

            MessageBoxResult resultDeleteConformation = MessageBox.Show(BuildMassege(selectedItems, messageParams), "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultDeleteConformation == MessageBoxResult.Yes)
            {
                foreach (SystemFileItem item in selectedItems)
                {
                    item.Copy(DirectoryToCopy);
                }
            }
            RefreshVisibleItems();
        }

        public void Create(string dirName)
        {
            CurrentItem.Create(dirName);
            RefreshVisibleItems();
        }

        public void Rename(string newName)
        {
            if (SelectedItem != null)
            {
                SelectedItem.Rename(newName);
            }
            RefreshVisibleItems();
        }

        /// <summary>
        /// Build massege for user actions
        /// </summary>
        /// <param name="message">massege</param>
        /// <param name="selectedItems">selectedItems</param>
        /// <returns></returns>
        private string BuildMassege(IList<SystemFileItem> selectedItems, params string[] message)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(message[0]);
            foreach (var item in selectedItems)
            {
                builder.AppendLine(item.Name + item.Ext);
            }
            for (int i = 1; i < message.Length; i++)
            {
                builder.Append(message[i]);
                builder.Append(" ");
            }

            return builder.ToString();
        }

    }
}
