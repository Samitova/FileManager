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
            Date = dir.LastAccessTime.ToString("dd.MM.yy HH:mm");
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

        public override void Rename(string newName)
        {
            string newDirName = System.IO.Path.Combine(Parent, newName);
            if (Path == newDirName) return;
            RenameDir(newDirName);
        }

        /// <summary>
        /// Move directory
        /// </summary>
        /// <param name="targetDir">target dir</param>
        public override void Move(string targetDir)
        {
            string pathToCopy = System.IO.Path.Combine(targetDir, Name + Ext);
            RenameDir(pathToCopy);
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
                        Directory.Delete(newName);
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
    }
}
