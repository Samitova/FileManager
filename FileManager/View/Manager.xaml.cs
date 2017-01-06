using FileManager.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
        
        private ManagerVievModel _managerVM;      
        private Pane FocusedPane;     

        public Manager()
        {
            InitializeComponent();

            _managerVM = new ManagerVievModel();
            DataContext = _managerVM;
        
            _leftPane.DataContext = _managerVM.LeftPaneViewModel;
            _rightPane.DataContext = _managerVM.RightPaneViewModel;         
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
                //FocusedPane.PaneVM.Copy();
                RefreshView();
            }
            if (e.Key == Key.F6)
            {
                //FocusedPane.PaneVM.Move();
                //RefreshView();
            }
            if (e.Key == Key.F7)
            {
                //string name = GetCreatingFileName();
                //if (name != null)
                //{
                //    FocusedPane.PaneVM.Create(name);
                //    RefreshView();
                //}
            }
            if (e.Key == Key.F8)
            {
                //FocusedPane.PaneVM.Delete();
                //RefreshView();
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

       


        private void SetCurrentFocusedPane(object sender, EventArgs e)
        {
            FocusedPane = (Pane)sender;
            _managerVM.CurrentPaneViewModel = FocusedPane.PaneVM;            
        }
    }
}
