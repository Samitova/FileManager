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
    /// Логика взаимодействия для SearchView.xaml
    /// </summary>
    public partial class SearchView : Window
    {
        internal SearchViewModel SearchVM;

        public SearchView()
        {
            InitializeComponent();            
            this.Loaded += new RoutedEventHandler(ViewLoaded);
        }

        private void ViewLoaded(object sender, RoutedEventArgs e)
        {
            SearchVM = this.DataContext as SearchViewModel;
            
        }

        private void filesListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SearchVM.GoToSelectedFile();
        }

        private void filesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (filesListBox.SelectedItem != null && SearchVM != null)
            {
                SearchVM.SelectedFoundFile = filesListBox.SelectedItem as string;                
            }
        }
    }
}
