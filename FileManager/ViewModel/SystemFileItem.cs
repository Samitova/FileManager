using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileManager.ViewModel
{
    abstract class SystemFileItem : DependencyObject
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Root { get; set; }
        public string Size { get; set; }
        public string Ext { get; set; }
        public string Date { get; set; }
        public string Icon { get; set; }

        //public static readonly DependencyProperty childrenProperty = DependencyProperty.Register("Children", typeof(IList<SystemFileItem>), typeof(SystemFileItem));
        //public IList<MyDirInfo> SubDirectories
        //{
        //    get { return (IList<SystemFileItem>)GetValue(childrenProperty);}
        //    set { SetValue(childrenProperty, value); }
        //}

        public static readonly DependencyProperty propertyIsSelected = DependencyProperty.Register("IsSelected", typeof(bool), typeof(SystemFileItem));
        public bool IsSelected
        {
            get { return (bool)GetValue(propertyIsSelected); }
            set { SetValue(propertyIsSelected, value); }
        }
    }
}
