using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.ViewModel
{
    interface IAction
    {
        void Create();
        void Delete();
        void Move(string targetDir);
        void Copy(string targetDir);
        void GetDetails();
        void Rename(string newName);
        
    }
}
