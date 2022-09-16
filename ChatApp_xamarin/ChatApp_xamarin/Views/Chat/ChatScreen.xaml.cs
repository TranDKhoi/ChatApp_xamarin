using ChatApp_xamarin.Models;
using ChatApp_xamarin.Utils;
using ChatApp_xamarin.ViewModels.Chat;
using ChatApp_xamarin.ViewModels.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChatApp_xamarin.Views.Chat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatScreen : ContentPage
    {
        public ChatScreen()
        {
            InitializeComponent();

        }
        protected override async void OnAppearing()
        {
            var viewModel = (ChatViewModel)this.BindingContext;
            await Task.Run(() => viewModel.InitCM.Execute(null));
            await Task.Run(() => viewModel.SubscribeMessageChange.Execute(null));

            Device.BeginInvokeOnMainThread(() =>
            {
                if (viewModel.ListMessage.Count == 0) return;
                MessageList.ScrollTo(viewModel.ListMessage.Last(), null, ScrollToPosition.End, true);
            });

            var sendMessage = new TapGestureRecognizer();
            sendMessage.Tapped += (s, e) =>
            {
                viewModel.SendMessageCM.Execute(null);
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (viewModel.ListMessage.Count == 0) return;
                    MessageList.ScrollTo(viewModel.ListMessage.Last(), null, ScrollToPosition.End, true);
                });
            };
            SendIcon.GestureRecognizers.Add(sendMessage);
        }
    }
}