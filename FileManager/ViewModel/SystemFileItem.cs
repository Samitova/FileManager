using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileManager.ViewModel
{
    abstract class SystemFileItem : DependencyObject, IAction
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Parent { get; set; }
        public string Size { get; set; }
        public string Ext { get; set; }
        public string CreationDate { get; set; }
        public string LastAcssesDate { get; set; }
        public string Icon { get; set; }
       

        public virtual void Delete()
        { }

        public virtual void Move(string targetDir)
        { }

        public virtual void Copy(string targetDir)
        { }

        public virtual void Rename(string newName)
        { }

        public virtual void GetDetails()
        { }

        public virtual void Execute()
        { }

        public virtual void Create(string dirName)
        { }
    }
}
