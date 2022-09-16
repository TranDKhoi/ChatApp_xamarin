using ChatApp_xamarin.ViewModels.Chat;
using ChatApp_xamarin.ViewModels.Friends;
using System;
using System.Collections.Generic;
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
    }
}