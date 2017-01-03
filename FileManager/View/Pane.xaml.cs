using FileManager.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileManager.View
{
    /// <summary>
    /// Логика взаимодействия для Pane.xaml
    /// </summary>
    public partial class Pane : UserControl
    {
        internal PaneViewModel _pane;

        public static RoutedEvent DoubleClickEvent { get; internal set; }

        public Pane()
        {
            _pane = new PaneViewModel();
            DataContext = _pane;
            InitializeComponent();            
        }

        private void dataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.F4)
            //{
            //    MessageBox.Show("haha");
            //}
            if (e.Key == Key.F5)
            {
                _pane.Copy(dataGrid.SelectedItems.Cast<SystemFileItem>().ToList());
            }
            //if (e.Key == Key.F6)
            //{
            //    MessageBox.Show("haha");
            //}
            //if (e.Key == Key.F7)
            //{
            //    MessageBox.Show("haha");
            //}
            if (e.Key == Key.F8)
            {
                _pane.Delete(dataGrid.SelectedItems.Cast<SystemFileItem>().ToList());               
            }
            //if (e.Key == Key.F9)
            //{
            //    MessageBox.Show("haha");
            //}
            //if (e.Key == Key.F10)
            //{
            //    MessageBox.Show("haha");
            //}
        }
        public event EventHandler SetFocusedExplorer;

        public void OnSetFocusedDataGrid(EventArgs e)
        {
            var handler = SetFocusedExplorer;
            if (handler != null) handler(this, e);
        }

      
        private void dataGrid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _pane.SelectedItem = (SystemFileItem)dataGrid.CurrentItem;
        }

        public void SetCopyDir(string path)
        {
            _pane.DirectoryToCopy = path;
        }

        /// <summary>
        /// File name validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            
            foreach (var change in from change in e.Changes
                                   from character in textBox.Text.Substring(change.Offset, change.AddedLength)
                                   .Where(character => System.IO.Path.GetInvalidFileNameChars().Contains(character))
                                   select change)
            textBox.Text = textBox.Text.Remove(change.Offset, change.AddedLength);
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.Key == Key.Enter)
            {               
                string newFileName = ((TextBox)sender).Text;                
                _pane.Rename(newFileName);
            }
        }

        private void dataGrid_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGrid.CurrentItem != null)
            {
                _pane.OpenCurrentItem();
            }           
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _pane.SelectedItems = dataGrid.SelectedItems.Cast<SystemFileItem>().ToList();
        }
    }
}
