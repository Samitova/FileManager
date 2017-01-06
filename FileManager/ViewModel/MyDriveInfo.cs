using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using FileManager.Model;

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

        public override int TotalSubFilesCount
        {
            get
            {
                try
                {
                    return (new DirectoryInfo(Path).GetFiles("*", SearchOption.AllDirectories)).Count();
                }
                catch (UnauthorizedAccessException e)
                {
                    MessageBox.Show(e.Message.ToString());
                }
                return -1;
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

        public override void Copy(string targetDir)
        {
            throw new NotSupportedException();
        }

        public override void Delete()
        {
            throw new NotSupportedException();
        }

        public override void Execute()
        {
            throw new NotSupportedException();
        }

        public override void GetDetails()
        {
            throw new NotSupportedException();
        }

        public override void Move(string targetDir)
        {
            throw new NotSupportedException();
        }

        public override void Rename(string newName)
        {
            throw new NotSupportedException();
        }

        public override List<SystemFileItem> GetChildren()
        {
            IList<SystemFileItem> childrenDirList = new List<SystemFileItem>();

            childrenDirList = childrenDirList.Concat(FileSystemProvider.GetChildrenDirectories(Path).Select(dir => new MyDirInfo(dir)).ToList<SystemFileItem>()).ToList();

            return childrenDirList.Concat(FileSystemProvider.GetChildrenFiles(Path).Select(dir => new MyFileInfo(dir)).ToList<SystemFileItem>()).ToList();
        }
    }
}
