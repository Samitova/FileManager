using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using FileManager.Model;
using System.Security;

namespace FileManager.ViewModel
{
    class MyDirInfo : SystemFileItem
    {
        private int SubFoldersCount
        {
            get
            {
                try
                {
                    return (new DirectoryInfo(Path).GetDirectories()).Count();
                }
                catch (UnauthorizedAccessException e)
                {
                    MessageBox.Show(e.Message.ToString());
                }
                return -1;
            }
        }

        private int SubFilesCount
        {
            get
            {
                try
                {
                    return (new DirectoryInfo(Path).GetFiles("*", SearchOption.TopDirectoryOnly)).Count();
                }
                catch (UnauthorizedAccessException e)
                {
                    MessageBox.Show(e.Message.ToString());
                }
                return -1;
            }
        }

        private string DirTotalSize
        {
            get
            {
                try
                {
                    return (new DirectoryInfo(Path).GetFiles("*", SearchOption.AllDirectories).Sum(file => file.Length) / 1024).ToString() + " KB";
                }
                catch (UnauthorizedAccessException e)
                {
                    MessageBox.Show(e.Message.ToString());
                }
                return string.Empty;
            }
        }

        public MyDirInfo(DirectoryInfo dir)
        {
            Name = dir.Name;
            Parent = dir.Parent.FullName;
            Path = dir.FullName;
            Size = "<dir>";
            Ext = "";
            LastWriteDate = dir.LastWriteTime.ToString("dd.MM.yy HH:mm");
            CreationDate = dir.CreationTime.ToString("dd.MM.yy HH:mm");
            LastAcssesDate = dir.LastAccessTime.ToString("dd.MM.yy HH:mm");
            Icon = @"Images/folder.png";
        }

        /// <summary>
        /// Copy directory
        /// </summary>
        /// <param name="targetDir"></param>
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
        /// Copy logic for directory with subdirectories
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

            builder.AppendLine(string.Format("{0, -30} {1, -15}", "Directory Name:", Name));
            builder.AppendLine(string.Format("{0, -36} {1, -15}", "Location:", Parent));
            builder.AppendLine(string.Format("{0, -40} {1, -15}", "Size:", DirTotalSize));
            builder.AppendLine(string.Format("{0, -32} {1, -15}", "Creation Date:", CreationDate));
            builder.AppendLine(string.Format("{0, -30} {1, -15}", "Last Acsses Date:", LastAcssesDate));
            builder.AppendLine(string.Format("{0, -32} {1, -15}", "Last Write Date:", LastWriteDate));
            builder.AppendLine(string.Format("{0, -28} {1, -15}", "Number of Folders:", SubFoldersCount));
            builder.AppendLine(string.Format("{0, -31} {1, -15}", "Number of Files:", SubFoldersCount));

            MessageBox.Show(builder.ToString(), "Directory details");
        }

        /// <summary>
        /// Create directory
        /// </summary>
        /// <param name="dirName">name</param>
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

        /// <summary>
        /// Delete directory
        /// </summary>
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

        /// <summary>
        /// Rename directory
        /// </summary>
        /// <param name="newName"></param>
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

        public override void Execute()
        { }

        public override List<SystemFileItem> GetChildren()
        {
            IList<SystemFileItem> childrenDirList = new List<SystemFileItem>();

            Name = "[...]";
            childrenDirList.Add(this);

            childrenDirList = childrenDirList.Concat(FileSystemProvider.GetChildrenDirectories(Path).Select(dir => new MyDirInfo(dir)).ToList<SystemFileItem>()).ToList();

            return childrenDirList.Concat(FileSystemProvider.GetChildrenFiles(Path).Select(dir => new MyFileInfo(dir)).ToList<SystemFileItem>()).ToList();
        }
    }
}
