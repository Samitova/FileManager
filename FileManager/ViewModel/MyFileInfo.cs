using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.ViewModel
{
    class MyFileInfo: SystemFileItem
    {
        public MyFileInfo(FileInfo file)
        {
            Name = System.IO.Path.GetFileNameWithoutExtension(file.Name);
            Path = file.FullName;            
            Size = (file.Length / 1024).ToString() + " KB";
            Ext = file.Extension;
            Date = file.LastAccessTime.ToString("dd.MM.yy HH:mm");
            Icon = "Images/file.png";
        }
    }
}
