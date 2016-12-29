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

namespace FileManager.View
{
    /// <summary>
    /// Логика взаимодействия для Pane.xaml
    /// </summary>
    public partial class Pane : UserControl
    {
        internal PaneViewModel _pane;

        public Pane()
        {
            _pane = new PaneViewModel();
            DataContext = _pane;
            InitializeComponent();            
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _pane.OpenCurrentItem(dataGrid.CurrentItem as SystemFileItem);
        }
    }
}
