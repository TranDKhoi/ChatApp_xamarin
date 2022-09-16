using ChatApp_xamarin.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using System.Linq;
using ChatApp_xamarin.Utils;

namespace ChatApp_xamarin.ViewModels.Converter
{
    public class MemberToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<User> users = value as List<User>;
            if (users.Count == 2)
            {
                var partner = users.Where(u => u.id != GlobalData.ins.currentUser.id).First();
                return partner.name;
            }
            return "Group";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<User> users = value as List<User>;
            if (users.Count == 2)
            {
                var partner = users.Where(u => u.id != GlobalData.ins.currentUser.id).First();
                return partner.name;
            }
            return "Group";
        }
    }

    public class MemberToAvatarConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<User> users = value as List<User>;
            if (users.Count == 2)
            {
                var partner = users.Where(u => u.id != GlobalData.ins.currentUser.id).First();
                return partner.avatar;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<User> users = value as List<User>;
            if (users.Count == 2)
            {
                var partner = users.Where(u => u.id != GlobalData.ins.currentUser.id).First();
                return partner.avatar;
            }
            return null;
        }
    }
}
