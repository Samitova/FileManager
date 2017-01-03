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
        void Delete();
        void Move(string targetDir);
        void Copy(string targetDir);
        void Rename(string newName);
        void GetDetails();  
    }
}
