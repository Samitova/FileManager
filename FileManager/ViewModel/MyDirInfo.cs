using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileManager.ViewModel
{
    class MyDirInfo : SystemFileItem
    {
        public MyDirInfo(DirectoryInfo dir)
        {
            Name = dir.Name;
            Root = dir.Root.Name;
            Path = dir.FullName;
            Size = "<dir>";           
            Ext = "";
            Date = dir.LastAccessTime.ToString("dd.MM.yy HH:mm");
            Icon = @"Images/folder.png";
        }
    }
}
