using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace FileManager.ViewModel
{
    class MyDriveInfo:SystemFileItem
    {
        public MyDriveInfo(string name)
        {           
            Name = name;
        }

        public MyDriveInfo(DriveInfo drive)
        {
            Name = drive.Name;
            Path = drive.Name;
        }

        public override void Create(string dirName)
        {
            string pathToCreate = System.IO.Path.Combine(Path, dirName);
            try
            {
                if (Directory.Exists(pathToCreate))
                {
                    MessageBoxResult resultConformation = MessageBox.Show("Folder with such name is allready existed. Do you want to replace it?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (resultConformation == MessageBoxResult.Yes)
                    {
                        Directory.Delete(pathToCreate, true);
                        Directory.CreateDirectory(pathToCreate);
                    }
                }
                else
                {
                    Directory.CreateDirectory(pathToCreate);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
