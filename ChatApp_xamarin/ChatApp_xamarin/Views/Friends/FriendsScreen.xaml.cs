using ChatApp_xamarin.Models;
using ChatApp_xamarin.Services;
using ChatApp_xamarin.Utils;
using ChatApp_xamarin.ViewModels.Chat;
using ChatApp_xamarin.ViewModels.Friends;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChatApp_xamarin.Views.Friends
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FriendsScreen : ContentPage
    {
        public FriendsScreen()
        {
            InitializeComponent();
        }

        private void listFriends_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = (FriendsViewModel)this.BindingContext;
            viewModel.OpenChatScreenVM.Execute(null);
            listFriends.SelectedItem = null;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //var viewModel = (FriendsViewModel)this.BindingContext;
            //viewModel.users = new ObservableCollection<User>(await UserService.ins.GetOnlineFriend(GlobalData.ins.currentUser.friendId));
        }
    }
}