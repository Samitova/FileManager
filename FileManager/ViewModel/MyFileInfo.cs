using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileManager.ViewModel
{
    class MyFileInfo : SystemFileItem, IAction
    {
        public string Dir { get; set; }
        public bool IsReadOnly
        {
            get
            {
                FileAttributes attributes = File.GetAttributes(Path);
                return (attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
            }
        }

        public MyFileInfo(FileInfo file)
        {
            Name = System.IO.Path.GetFileNameWithoutExtension(file.Name);
            Dir = file.DirectoryName;
            Path = file.FullName;
            Size = (file.Length / 1024).ToString() + " KB";
            Ext = file.Extension;
            Date = file.LastAccessTime.ToString("dd.MM.yy HH:mm");
            Icon = "Images/file.png";

        }

        public override void Copy(string targetDir)
        {
            string pathToCopy = System.IO.Path.Combine(targetDir, Name + Ext);

            try
            {
                if (File.Exists(pathToCopy))
                {
                    MessageBoxResult resultConformation = MessageBox.Show("File with such name is allready existed. Do you want to rewrite it?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (resultConformation == MessageBoxResult.Yes)
                    {
                        File.SetAttributes(pathToCopy, FileAttributes.Normal);
                        File.Delete(pathToCopy);
                        File.Copy(Path, pathToCopy);
                    }
                }
                else
                {
                    File.Copy(Path, pathToCopy);
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

        public override void Create()
        {

        }

        public override void Delete()
        {
            try
            {
                if (IsReadOnly)
                {
                    MessageBoxResult resultReadOnlyConformation = MessageBox.Show(Name + Ext + " is readonly. Do you want to delete?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (resultReadOnlyConformation == MessageBoxResult.Yes)
                    {
                        File.SetAttributes(Path, FileAttributes.Normal);
                        File.Delete(Path);
                    }
                }
                else
                {
                    File.Delete(Path);
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
        /// Move file
        /// </summary>
        /// <param name="targetDir">target dir</param>
        public override void Move(string targetDir)
        {
            string pathToCopy = System.IO.Path.Combine(targetDir, Name + Ext);
            RenameFile(pathToCopy);
        }

        /// <summary>
        /// Rename file
        /// </summary>
        /// <param name="newName">new name</param>
        public override void Rename(string newName)
        {
            string newFileName = System.IO.Path.Combine(Dir, newName + Ext);
            if (Path == newFileName) return;
            RenameFile(newFileName);
        }

        /// <summary>
        /// Rename/move file logic
        /// </summary>
        /// <param name="newName">newName</param>
        private void RenameFile(string newName)
        {
            try
            {
                if (File.Exists(newName))
                {
                    MessageBoxResult resultConformation = MessageBox.Show("File with such name is allready existed. Do you want to rewrite it?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (resultConformation == MessageBoxResult.Yes)
                    {
                        File.SetAttributes(newName, FileAttributes.Normal);
                        File.Delete(newName);
                        File.Move(Path, newName);
                    }
                }
                else
                {
                    File.Move(Path, newName);
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
        /// Get details of file
        /// </summary>
        public override void GetDetails()
        { }

        public override void Execute()
        {
            System.Diagnostics.Process.Start(this.Path);
        }
    }
}
