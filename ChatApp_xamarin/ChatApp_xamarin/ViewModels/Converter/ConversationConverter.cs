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
    public class RoomNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Room room = value as Room;
            if (room != null)
            {
                if (room.member.Count == 2)
                {
                    var partner = room.member.Where(u => u.id != GlobalData.ins.currentUser.id).First();
                    return partner.name;
                }
                return room.roomName;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Room room = value as Room;
            if (room != null)
            {
                if (room.member.Count == 2)
                {
                    var partner = room.member.Where(u => u.id != GlobalData.ins.currentUser.id).First();
                    return partner.name;
                }
                return room.roomName;
            }
            return null;
        }
    }

    public class RoomAvatarConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            Room room = value as Room;
            if (room != null)
            {
                if (room.member.Count == 2)
                    return room.member.Where(u => u.id != GlobalData.ins.currentUser.id).First().avatar;
                else
                    return room.avatar;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Room room = value as Room;
            if (room != null)
            {
                if (room.member.Count == 2)
                    return room.member.Where(u => u.id != GlobalData.ins.currentUser.id).First().avatar;
                else
                    return room.avatar;
            }
            return null;
        }
    }

    public class IsOnlineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isOnline = (bool)value;
            return (isOnline) ? Color.FromHex("#FF50ca30") : Color.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isOnline = (bool)value;
            return (isOnline) ? Color.FromHex("#FF50ca30") : Color.Red;
        }
    }

    public class IsOnlineRoomConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var members = value as List<User>;
            if (members.Count == 2)
            {
                for (int i = 0; i < members.Count; i++)
                {
                    if (members[i].id != GlobalData.ins.currentUser.id)
                    {
                        return (members[i].isOnline) ? Color.FromHex("#FF50ca30") : Color.Red;
                    }
                }
            }
            return Color.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var members = value as List<User>;
            if (members.Count == 2)
            {
                for (int i = 0; i < members.Count; i++)
                {
                    if (members[i].id != GlobalData.ins.currentUser.id)
                    {
                        return (members[i].isOnline) ? Color.FromHex("#FF50ca30") : Color.Red;
                    }
                }
            }
            return Color.Gray;
        }
    }

    public class TimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string time = value as string;
            if (string.IsNullOrEmpty(time))
            {
                return "";
            }
            else
            {
                DateTime d = DateTime.Parse(time, System.Globalization.CultureInfo.InvariantCulture);
                return d.ToString("hh:mm");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string time = value as string;
            if (string.IsNullOrEmpty(time))
            {
                return "";
            }
            else
            {
                DateTime d = DateTime.Parse(time);
                return d.ToString("hh:mm");
            }
        }
    }
}
