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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal PaneViewModel _leftPane;
        internal PaneViewModel _rightPane;

        public MainWindow()
        {
            _leftPane = new PaneViewModel();
            _rightPane = new PaneViewModel(@"d:\");
            InitializeComponent();
            Loaded += new RoutedEventHandler(LeftPane_Loaded);
            Loaded += new RoutedEventHandler(RightPane_Loaded);
        }

      

        private void LeftPane_Loaded(object sender, RoutedEventArgs e)
        {
            LeftPane.DataContext = _leftPane;
        }

        private void RightPane_Loaded(object sender, RoutedEventArgs e)
        {
            RightPane.DataContext = _rightPane;
        }
    }
}
