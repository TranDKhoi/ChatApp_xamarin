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
        protected override void OnAppearing()
        {
            var viewModel = (ChatViewModel)this.BindingContext;
            viewModel.InitCM.Execute(MessageList);
        }
    }
}