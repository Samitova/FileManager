using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
    }
}
