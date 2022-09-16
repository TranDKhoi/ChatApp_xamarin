﻿using ChatApp_xamarin.Models;
using ChatApp_xamarin.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace ChatApp_xamarin.ViewModels.Chat
{
    internal class MessageItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ReceiveTemplate { get; set; }
        public DataTemplate SendTemplate { get; set; }
        public DataTemplate SendImageTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item == null) return null;
            Message messageItem = item as Message;
            if (messageItem.images != null)
            {
                return SendImageTemplate;
            }
            else
            {
                if (messageItem.senderId == GlobalData.ins.currentUser.id)
                {
                    return SendTemplate;
                }
                return ReceiveTemplate;
            }
        }
    }
}
