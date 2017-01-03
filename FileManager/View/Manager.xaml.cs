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
        public Manager()
        {           
            InitializeComponent();           
        }
     
        private void LeftPane_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {           
            LeftPane.SetCopyDir(RightPane._pane.CurrentItem.Path);
        }

        private void RightPane_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RightPane.SetCopyDir(LeftPane._pane.CurrentItem.Path);
        }
    }
}
