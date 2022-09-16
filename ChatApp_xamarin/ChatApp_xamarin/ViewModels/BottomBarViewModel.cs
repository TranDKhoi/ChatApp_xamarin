using ChatApp_xamarin.Models;
using ChatApp_xamarin.Services;
using ChatApp_xamarin.Utils;
using ChatApp_xamarin.ViewModels.Chat;
using Plugin.LocalNotification.AndroidOption;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ChatApp_xamarin.ViewModels
{
    public class BottomBarViewModel : BaseViewModel
    {

        public ICommand SubscribeToConversationCM { get; set; }
        public ICommand UnSubscribeToConversationCM { get; set; }


        private IDisposable conversationListener;

        private bool isSubscribeConver { get; set; }
        private bool isFirstTime { get; set; }

        public BottomBarViewModel()
        {

            isFirstTime = true;

            SubscribeToConversationCM = new Command(() =>
            {
                if (isSubscribeConver == true) return;

                var subscriber = ConversationService.ins.SubscriptionToConversationChange();
                conversationListener = subscriber.Subscribe(item =>
                {
                    RefreshConversationAndNotify(item.Object);
                });

                isSubscribeConver = true;
            });

            UnSubscribeToConversationCM = new Command(() =>
            {
                isSubscribeConver = false;
                isFirstTime = true;
                if (conversationListener != null)
                    conversationListener.Dispose();
            });

        }
        private void RefreshConversationAndNotify(Room item)
        {
            if (!GlobalData.ins.currentUser.roomKey.Contains(item.id)) return;

            var converVM = Application.Current.Resources["ConversationVM"] as ConversationViewModel;
            converVM.GetAllConversation.Execute(null);

            showNotification(item);
        }

        private async void showNotification(Room item)
        {
            if (isFirstTime)
            {
                isFirstTime = false;
                return;
            }

            //nếu đang mở màn hình chat ở room này thì ko báo
            var ChatVM = Application.Current.Resources["ChatVM"] as ChatViewModel;

            if (ChatVM.CurrentRoom != null)
                if (ChatVM.CurrentRoom.id == item.id)
                    return;


            var sender = await UserService.ins.GetUserById(item.lastMessage.senderId);

            var notification = new NotificationRequest
            {
                BadgeNumber = 3,
                Description = item.lastMessage.message,
                Title = sender.name,
                //ReturningData = "Test Data",
                NotificationId = 1,
                Android = new AndroidOptions
                {
                    ChannelId = "AndroidMessChannel",
                    LaunchAppWhenTapped = true,
                    Priority = AndroidPriority.Max,
                }
            };

            _ = LocalNotificationCenter.Current.Show(notification);
        }
    }
}
