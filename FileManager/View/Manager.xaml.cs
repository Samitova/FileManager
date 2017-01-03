using FileManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FileManager.View
{
    /// <summary>
    /// Логика взаимодействия для Manager.xaml
    /// </summary>
    public partial class Manager : Window
    {
        private PaneViewModel _leftPaneViewModel;
        private PaneViewModel _rightPaneViewModel;
        private Pane FocusedPane;     

        public Manager()
        {
            _leftPaneViewModel = new PaneViewModel();
            _rightPaneViewModel = new PaneViewModel();
            InitializeComponent();
            _leftPane.DataContext = _leftPaneViewModel;
            _rightPane.DataContext = _rightPaneViewModel;
        }

        private void LeftPane_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _leftPane.DirectoryToCopy = _rightPane.GetCurrentPath();            
        }

        private void RightPane_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _rightPane.DirectoryToCopy = _leftPane.GetCurrentPath();
        }

        private void OuterGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                FocusedPane.PaneVM.Copy();
                RefreshView();
            }
            if (e.Key == Key.F6)
            {
                FocusedPane.PaneVM.Move();
                RefreshView();
            }
            if (e.Key == Key.F7)
            {
                string name = GetCreatingFileName();
                if (name != null)
                {
                    FocusedPane.PaneVM.Create(name);
                    RefreshView();
                }
            }
            if (e.Key == Key.F8)
            {
                FocusedPane.PaneVM.Delete();
                RefreshView();
            }
            if (e.Key == Key.F9)
            {
                FocusedPane.PaneVM.GetDetails();
            }
        }

        /// <summary>
        /// Refresh view of dirs and files
        /// </summary>
        private void RefreshView()
        {
            _rightPane.PaneVM.RefreshVisibleItems();
            _leftPane.PaneVM.RefreshVisibleItems();
        }

        /// <summary>
        /// Get creating directory name
        /// </summary>
        /// <returns></returns>
        private string GetCreatingFileName()
        {
            CreateDirectory createDirWindow = new CreateDirectory();
            createDirWindow.Owner = this;
            createDirWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            createDirWindow.ShowDialog();
            return createDirWindow.DirName;
        }

        private void copyButton_Click(object sender, RoutedEventArgs e)
        {
            FocusedPane.PaneVM.Copy();
            RefreshView();
        }

        private void moveButton_Click(object sender, RoutedEventArgs e)
        {
            FocusedPane.PaneVM.Move();
            RefreshView();
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            string name = GetCreatingFileName();
            if (name != null)
            {
                FocusedPane.PaneVM.Create(name);
                RefreshView();
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            FocusedPane.PaneVM.Delete();
            RefreshView();
        }

        private void detailsButton_Click(object sender, RoutedEventArgs e)
        {
            FocusedPane.PaneVM.GetDetails();
        }

        private void SetCurrentFocusedPane(object sender, EventArgs e)
        {
            FocusedPane = (Pane)sender;            
        }
    }
}
