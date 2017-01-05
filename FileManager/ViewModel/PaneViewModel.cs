using FileManager.Model;
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
        BackgroundWorker worker;

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
            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);

        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Copy2();
        }

        private void Copy2()
        {
           

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(200);
                
            }

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
        public void Move()
        {
            string[] messageParams = new string[] { "Do you want to move ", "to", DirectoryToCopy, "?" };
            MessageBoxResult resultDeleteConformation = MessageBox.Show(BuildMassege(SelectedItems, messageParams), "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultDeleteConformation == MessageBoxResult.Yes)
            {
                foreach (SystemFileItem item in SelectedItems)
                {
                    item.Move(DirectoryToCopy);
                }
            }            
        }

        /// <summary>
        /// Delete selected items
        /// </summary>
        /// <param name="selectedItems">selectedItems</param>
        public void Delete()
        {
            string[] messageParams = new string[] { "Do you want to delete ", "?" };

            MessageBoxResult resultDeleteConformation = MessageBox.Show(BuildMassege(SelectedItems, messageParams), "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resultDeleteConformation == MessageBoxResult.Yes)
            {
                foreach (SystemFileItem item in SelectedItems)
                {
                    item.Delete();
                }
            }            
        }

        /// <summary>
        /// Copy selected items
        /// </summary>
        /// <param name="selectedItems">selectedItems</param>
        public void Copy(Action<int> progress)
        {
            string[] messageParams = new string[] { "Do you want to copy ", "to", DirectoryToCopy, "?" };

            MessageBoxResult resultDeleteConformation = MessageBox.Show(BuildMassege(SelectedItems, messageParams), "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultDeleteConformation == MessageBoxResult.Yes)
            {
                int count = 0;
                foreach (SystemFileItem item in SelectedItems)
                {
                    item.Copy(DirectoryToCopy);
                    progress(++count);
                }
            }           
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
