using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace FileManager.ViewModel
{
    class MyDirInfo : SystemFileItem, IAction
    {
        public MyDirInfo(DirectoryInfo dir)
        {
            Name = dir.Name;
            Parent = dir.Parent.FullName;
            Path = dir.FullName;
            Size = "<dir>";
            Ext = "";
            CreationDate = dir.CreationTime.ToString("dd.MM.yy HH:mm");
            LastAcssesDate = dir.LastAccessTime.ToString("dd.MM.yy HH:mm");
            Icon = @"Images/folder.png";
        }

        public override void Copy(string targetDir)
        {
            string pathToCopy = System.IO.Path.Combine(targetDir, Name);
            try
            {
                if (Directory.Exists(pathToCopy))
                {
                    MessageBoxResult resultConformation = MessageBox.Show("Folder with such name is allready existed. Do you want to rewrite it?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (resultConformation == MessageBoxResult.Yes)
                    {
                        Directory.Delete(pathToCopy, true);
                        DirectoryCopy(Path, pathToCopy);
                    }
                }
                else
                {
                    DirectoryCopy(Path, pathToCopy);
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

        /// <summary>
        /// Copy directory with subdirectories
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        private void DirectoryCopy(string sourceDirName, string destDirName)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = System.IO.Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = System.IO.Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath);
            }
        }

        /// <summary>
        /// Get details of dir
        /// </summary>
        public override void GetDetails()
        {
            StringBuilder builder = new StringBuilder();
           
            builder.AppendLine(string.Format("{0, -20} {1, -15}", "Directory Name:", Name));
            builder.AppendLine(string.Format("{0, -26} {1, -15}", "Location:", Parent)); 
            builder.AppendLine(string.Format("{0, -30} {1, -15}", "Size:", CalculateDirSize()));
            builder.AppendLine(string.Format("{0, -22} {1, -15}", "Creation Date:", CreationDate)); 
            builder.AppendLine(string.Format("{0, -20} {1, -15}", "Last Acsses Date:", LastAcssesDate));            

            MessageBox.Show(builder.ToString(), "Directory details");
        }

        /// <summary>
        /// Calculate size of all files in directory
        /// </summary>
        /// <returns></returns>
        private string CalculateDirSize()
        {
            DirectoryInfo dir = new DirectoryInfo(Path);
            return (dir.GetFiles("*", SearchOption.AllDirectories).Sum(file => file.Length)/ 1024).ToString() + " KB";
        }

        public override void Delete()
        {
            try
            {
                Directory.Delete(Path, true);
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

        /// <summary>
        /// Move directory
        /// </summary>
        /// <param name="targetDir">target dir</param>
        public override void Move(string targetDir)
        {
            string pathToCopy = System.IO.Path.Combine(targetDir, Name);
            RenameDir(pathToCopy);
        }

        public override void Rename(string newName)
        {
            string newDirName = System.IO.Path.Combine(Parent, newName);
            if (Path == newDirName) return;
            RenameDir(newDirName);
        }

        /// <summary>
        /// Rename/move Directory logic
        /// </summary>
        /// <param name="newName">newName</param>
        private void RenameDir(string newName)
        {
            try
            {
                if (Directory.Exists(newName))
                {
                    MessageBoxResult resultConformation = MessageBox.Show("Folder with such name is allready existed. Do you want to replace it?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (resultConformation == MessageBoxResult.Yes)
                    {
                        Directory.Delete(newName, true);
                        Directory.Move(Path, newName);
                    }
                }
                else
                {
                    Directory.Move(Path, newName);
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
