using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace FileManager.ViewModel.Converters
{
  public class BooleanToVisibilityConverter : IValueConverter
  {
   
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {      
      if (value is bool == false)
        return null;
            
      bool boolean = (bool)value;
      
      if (parameter != null && parameter.ToString() == "Invert")
      {
        return boolean ? Visibility.Collapsed : Visibility.Visible;
      }
      else
      {
        return boolean ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
