using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace ChatApp_xamarin.ViewModels.Converter
{
    public class IconColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string icon = value as string;
            if (Application.Current.UserAppTheme == OSAppTheme.Light)
            {
                return icon + "_dark";
            }
            return icon + "_light";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string icon = value as string;
            if (Application.Current.UserAppTheme == OSAppTheme.Light)
            {
                return icon + "_dark";
            }
            return icon + "_light";
        }
    }
}
