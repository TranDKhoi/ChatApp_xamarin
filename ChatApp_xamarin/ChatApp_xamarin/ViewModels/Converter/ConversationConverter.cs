using ChatApp_xamarin.Models;
using ChatApp_xamarin.Resources;
using ChatApp_xamarin.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

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
                DateTime d = new DateTime();
                if (time.Contains("PM") || time.Contains("AM"))
                {
                    d = DateTime.Parse(time, new CultureInfo("en-US", false));
                }
                else if (time.Contains("SA") || time.Contains("CH"))
                {
                    d = DateTime.Parse(time, new CultureInfo("vi-VN", false));
                }
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
                DateTime d = new DateTime();
                if (time.Contains("PM") || time.Contains("AM"))
                {
                    d = DateTime.Parse(time, new CultureInfo("en-US", false));
                }
                else if (time.Contains("SA") || time.Contains("CH"))
                {
                    d = DateTime.Parse(time, new CultureInfo("vi-VN", false));
                }
                return d.ToString("hh:mm");
            }
        }
    }

    public class LassMessageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Message message = value as Message;

            if (message is null) return "";

            if (message.message != null)
                return message.message;
            if (message.image != null)
                return AppResources.newimage;
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Message message = value as Message;

            if (message is null) return "";

            if (message.message != null)
                return message.message;
            if (message.image != null)
                return AppResources.newimage;
            return "";
        }
    }
    public class IsSeenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<string> listId = value as List<string>;
            if (listId == null)
                return FontAttributes.Bold;
            if (!listId.Contains(GlobalData.ins.currentUser.id))
                return FontAttributes.Bold;
            return FontAttributes.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<string> listId = value as List<string>;
            if (listId == null)
                return FontAttributes.Bold;
            if (!listId.Contains(GlobalData.ins.currentUser.id))
                return FontAttributes.Bold;
            return FontAttributes.None;
        }
    }
    public class IsSeenToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<string> listId = value as List<string>;
            if (listId == null)
                return true;
            if (!listId.Contains(GlobalData.ins.currentUser.id))
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<string> listId = value as List<string>;
            if (listId == null)
                return true;
            if (!listId.Contains(GlobalData.ins.currentUser.id))
                return true;
            return false;
        }
    }
}
