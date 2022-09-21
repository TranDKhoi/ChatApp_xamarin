using ChatApp_xamarin.Models;
using ChatApp_xamarin.Services;
using ChatApp_xamarin.Utils;
using ChatApp_xamarin.ViewModels.Friends;
using System.Collections.ObjectModel;

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
            searchEntry.Background = null;
        }

        private void listFriends_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = (FriendsViewModel)this.BindingContext;
            viewModel.OpenChatScreenVM.Execute(listFriends.SelectedItem);
            listFriends.SelectedItem = null;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var viewModel = (FriendsViewModel)this.BindingContext;
            if (GlobalData.ins.currentUser != null)
            {
                if (GlobalData.ins.currentUser.friendId != null)
                {
                    if (await UserService.ins.GetOnlineFriend(GlobalData.ins.currentUser.friendId) != null)
                    {
                        viewModel.users = new ObservableCollection<User>(await UserService.ins.GetOnlineFriend(GlobalData.ins.currentUser.friendId));

                    }
                }
            }

        }
    }
}