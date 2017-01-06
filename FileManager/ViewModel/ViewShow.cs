using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FileManager.ViewModel
{
    class ViewShow
    {
        public static void Show(Control p_view)
        {
            if (p_view != null)
            {
                Window window = new Window();
                window.SizeToContent = SizeToContent.WidthAndHeight;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                StackPanel sp = new StackPanel();
                sp.Children.Add(p_view);

                window.Content = sp;

                window.Show();
            }
        }

    }
}
