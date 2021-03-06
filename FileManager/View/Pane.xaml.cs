﻿using FileManager.ViewModel;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace FileManager.View
{
    /// <summary>
    /// Логика взаимодействия для Pane.xaml
    /// </summary>
    public partial class Pane : UserControl
    {
        public event EventHandler SetFocusedPane;
        public event EventHandler NeedsUpdateSource;
       
        internal PaneViewModel PaneVM { get; set; }
        internal string DirectoryToCopy {          
            set
            {
                PaneVM.DirectoryToCopy = value;
            }
        }

        public void OnSetFocusedDataGrid(EventArgs e)
        {
            var handler = SetFocusedPane;
            if (handler != null) handler(this, e);
        }
       
        public void OnNeedsUpdateSource(EventArgs e)
        {
            var handler = NeedsUpdateSource;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// ctor
        /// </summary>
        public Pane()
        {           
            InitializeComponent();
            PaneVM = new PaneViewModel();           
            this.Loaded += new RoutedEventHandler(ViewLoaded);           
        }

        private void ViewLoaded(object sender, RoutedEventArgs e)
        {
           PaneVM = this.DataContext as PaneViewModel;          
           PaneVM.NeedsUpdateSource += (o, ea) => OnNeedsUpdateSource(ea);        
        }
        
        public string GetCurrentPath()
        {
            return PaneVM.CurrentItem.Path;
        } 

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OnSetFocusedDataGrid(e);

            if (dataGrid.SelectedItems != null && PaneVM != null)
            {
                PaneVM.SelectedItem = dataGrid.SelectedItem as SystemFileItem;
                PaneVM.SelectedItems = dataGrid.SelectedItems.Cast<SystemFileItem>().ToList();
            }           
        }

        /// <summary>
        /// Rename file on Enter down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string newFileName = ((TextBox)sender).Text;
                PaneVM.Rename(newFileName);
            }
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
            {
                textBox.Text = textBox.Text.Remove(change.Offset, change.AddedLength);
            }
        }

        /// <summary>
        /// Execute item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PaneVM.OpenCurrentItem();
        }

        private void driveBox_DropDownOpened(object sender, EventArgs e)
        {
            var index = driveBox.SelectedIndex;
            PaneVM.RefreshDrivers();
            driveBox.SelectedIndex = index;
        }               
    }
}
