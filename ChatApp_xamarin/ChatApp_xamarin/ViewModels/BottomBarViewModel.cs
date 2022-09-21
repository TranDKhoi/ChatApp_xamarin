using ChatApp_xamarin.Models;
using ChatApp_xamarin.Services;
using ChatApp_xamarin.Utils;
using ChatApp_xamarin.ViewModels.Chat;
using Firebase.Database.Streaming;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
using System;
using System.Linq;
using System.Threading.Tasks;
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
                    RefreshConversationAndNotify(item);
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
        private async void RefreshConversationAndNotify(FirebaseEvent<Room> item)
        {
            var converVM = Application.Current.Resources["ConversationVM"] as ConversationViewModel;
            converVM.GetAllConversation.Execute(null);

            await Task.Run(() => showNotification(item.Object));
        }

        private async void showNotification(Room item)
        {
            if (item is null) return;

            //nếu như đây là room mới
            if (item.lastMessage is null) return;

            //nếu đây là các room cũ
            if (string.Compare(item.lastMessage.createdAt, DateTime.Now.ToString()) < 0) return;

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

            User partner;
            if (item.roomName is null)
            {
                partner = await UserService.ins.GetUserById(item.memberId.Where(i => i != GlobalData.ins.currentUser.id).First());
                item.roomName = partner.name;
            }

            var notification = new NotificationRequest
            {
                BadgeNumber = 3,
                Description = item.lastMessage == null ? "" : item.lastMessage.message,
                Title = item.roomName,
                //ReturningData = "Test Data",
                NotificationId = 1,
                Android = new AndroidOptions
                {
                    ChannelId = "AndroidMessChannel",
                    LaunchAppWhenTapped = true,
                    Priority = AndroidPriority.Max,
                }
            };


            if (!GlobalData.ins.isSilentMode)
            {
                await LocalNotificationCenter.Current.Show(notification);
            }
        }
    }
}
