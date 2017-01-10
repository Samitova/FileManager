using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileManager.ViewModel
{
    abstract class SystemFileItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Parent { get; set; }
        public string Size { get; set; }
        public string Ext { get; set; }
        public string CreationDate { get; set; }
        public string LastAcssesDate { get; set; }
        public string LastWriteDate { get; set; }
        public string Icon { get; set; }

        public abstract int TotalSubFilesCount { get; }

        public abstract void Create(string dirName);

        public abstract void Delete();

        public abstract void Move(string targetDir);

        public abstract void Copy(string targetDir);

        public abstract void Rename(string newName);

        public abstract void GetDetails();

        public abstract void Execute();

        public abstract List<SystemFileItem> GetChildren();


        public override bool Equals(object obj)
        {
            if (obj is SystemFileItem)
            {
                if (obj != null && this.Path == ((SystemFileItem)obj).Path)
                    return true;
            }                   

            return false;              
        }

    }
}
