using FileManager.ViewModel;
using System;
using System.Windows;
using System.Windows.Input;

namespace FileManager.View
{
    public partial class Manager : Window
    {        
        private ManagerVievModel _managerVM;      
        private Pane FocusedPane;     

        public Manager()
        {
            InitializeComponent();

            _managerVM = new ManagerVievModel();
            DataContext = _managerVM;
            FocusedPane = _leftPane;
            _leftPane.DataContext = _managerVM.LeftPaneViewModel;
            _rightPane.DataContext = _managerVM.RightPaneViewModel;
        }

        /// <summary>
        /// Store dirrectory to copy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeftPane_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _leftPane.DirectoryToCopy = _rightPane.GetCurrentPath();            
        }

        /// <summary>
        /// Store dirrectory to copy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RightPane_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _rightPane.DirectoryToCopy = _leftPane.GetCurrentPath();
        }      

        /// <summary>
        /// Refresh view of dirs and files
        /// </summary>
        private void RefreshView(object sender, EventArgs e)
        {
            _rightPane.PaneVM.RefreshVisibleItems();
            _leftPane.PaneVM.RefreshVisibleItems();
        }

        /// <summary>
        /// Get current pane
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetCurrentFocusedPane(object sender, EventArgs e)
        {
            FocusedPane = (Pane)sender;
            _managerVM.CurrentPaneViewModel = FocusedPane.PaneVM;  
                
        }
    }
}
