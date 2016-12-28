using FileManager.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileManager.ViewModel
{
    internal class MyDirInfo : DependencyObject
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Root { get; set; }
        public string Size { get; set; }
        public string Ext { get; set; }
        public string Date { get; set; }
        public string Icon { get; set; }
        public int Type { get; set; }

        public static readonly DependencyProperty childrenProperty = DependencyProperty.Register("Children", typeof(IList<MyDirInfo>), typeof(MyDirInfo));
        public IList<MyDirInfo> SubDirectories
        {
            get { return (IList<MyDirInfo>)GetValue(childrenProperty);}
            set { SetValue(childrenProperty, value); }
        }

        public static readonly DependencyProperty propertyIsSelected = DependencyProperty.Register("IsSelected", typeof(bool), typeof(MyDirInfo));
        public bool IsSelected
        {
            get { return (bool)GetValue(propertyIsSelected); }
            set { SetValue(propertyIsSelected, value); }
        }

        public MyDirInfo()
        {
            SubDirectories = new List<MyDirInfo>();
            
        }

        public MyDirInfo(string directoryName)
        {
            Name = directoryName;
        }

        public MyDirInfo(DirectoryInfo dir): this()
        {
            Name = dir.Name;
            Root = dir.Root.Name;
            Path = dir.FullName;
            Size = "<dir>";
            Type = (int)MyDirectoryType.Directory;
            Ext = "";
            Date = dir.LastAccessTime.ToString("dd.MM.yy HH:mm");
            Icon = @"Images/folder.png";
        }

        public MyDirInfo(FileInfo file) 
        {
            Name = System.IO.Path.GetFileNameWithoutExtension(file.Name);
            Path = file.FullName;
            Type = (int)MyDirectoryType.File;
            Size = (file.Length / 1024).ToString() + " KB";
            Ext = file.Extension;
            Date = file.LastAccessTime.ToString("dd.MM.yy HH:mm");
            Icon = "Images/file.png";
        }

        public MyDirInfo(DriveInfo drive): this()
        {
            if (drive.Name.EndsWith(@"\"))
            {
                Name = drive.Name.Substring(0, drive.Name.Length - 1);
            }
            else
            {
                Name = drive.Name;
            }                

            Path = drive.Name;
            Type = (int)MyDirectoryType.Driver;
        }
    }
}
