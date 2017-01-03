using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileManager.ViewModel
{
    class MyFileInfo : SystemFileItem
    {        
        public bool IsReadOnly
        {
            get
            {
                FileAttributes attributes = File.GetAttributes(Path);
                return (attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
            }
        }

        public bool IsHidden
        {
            get
            {
                FileAttributes attributes = File.GetAttributes(Path);
                return (attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
            }
        }

        public MyFileInfo(FileInfo file)
        {
            Name = System.IO.Path.GetFileNameWithoutExtension(file.Name);
            Parent = file.DirectoryName;            
            Path = file.FullName;
            Size = (file.Length / 1024).ToString() + " KB";
            Ext = file.Extension;
            LastAcssesDate = file.LastAccessTime.ToString("dd.MM.yy HH:mm");
            CreationDate = file.CreationTime.ToString("dd.MM.yy HH:mm");
            Icon = "Images/file.png";
        }

        /// <summary>
        /// Copy file
        /// </summary>
        /// <param name="targetDir"></param>
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

        /// <summary>
        /// Delete file
        /// </summary>
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
            string newFileName = System.IO.Path.Combine(Parent, newName + Ext);
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
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(string.Format("{0, -25} {1, -15}", "File Name:", Name+Ext));
            builder.AppendLine(string.Format("{0, -26} {1, -15}", "Location:", Parent));
            builder.AppendLine(string.Format("{0, -30} {1, -15}", "Size:", Size));
            builder.AppendLine(string.Format("{0, -22} {1, -15}", "Creation Date:", CreationDate));
            builder.AppendLine(string.Format("{0, -20} {1, -15}", "Last Acsses Date:", LastAcssesDate));
            builder.AppendLine(string.Format("{0, -24} {1, -15}", "IsReadOnly:", IsReadOnly));
            builder.AppendLine(string.Format("{0, -26} {1, -15}", "IsHidden:", IsHidden));

            MessageBox.Show(builder.ToString(), "File Details");
        }

        public override void Execute()
        {
            System.Diagnostics.Process.Start(this.Path);
        }
    }
}
