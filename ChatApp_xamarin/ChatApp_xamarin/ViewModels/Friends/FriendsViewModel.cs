using ChatApp_xamarin.Models;
using ChatApp_xamarin.ViewModels.Chat;
using ChatApp_xamarin.Views.Chat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ChatApp_xamarin.ViewModels.Friends
{
    public class FriendsViewModel : BaseViewModel
    {
        public ICommand OpenChatScreenVM { get; set; }
        private ObservableCollection<User> _users;
        public ObservableCollection<User> users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        public FriendsViewModel()
        {
            
            OpenChatScreenVM = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ChatScreen());
            });
        }
    }
}
