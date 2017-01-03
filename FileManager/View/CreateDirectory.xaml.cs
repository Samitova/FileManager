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
    /// Логика взаимодействия для CreateDirectory.xaml
    /// </summary>
    public partial class CreateDirectory : Window
    {
        public string DirName { get; set; }

        public CreateDirectory()
        {
            InitializeComponent();
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
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

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            DirName = textBox.Text;
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DirName = null;
            this.Close();
        }
    }
}
