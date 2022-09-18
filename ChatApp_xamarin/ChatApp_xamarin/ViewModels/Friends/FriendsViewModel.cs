using ChatApp_xamarin.Models;
using ChatApp_xamarin.Services;
using ChatApp_xamarin.Utils;
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
        public ICommand SearchCM { get; set; }
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

        private string _searchName;
        public string searchName
        {
            get { return _searchName; }
            set
            {
                _searchName = value;
                OnPropertyChanged();
            }
        }

        public FriendsViewModel()
        {

            SearchCM = new Command(async () =>
            {
                users = new ObservableCollection<User>(await UserService.ins.GetUserByName(searchName));
            });

            OpenChatScreenVM = new Command(async (p) =>
            {
                ChatScreen chatScreen = new ChatScreen();
                var vm = (ChatViewModel)chatScreen.BindingContext;
                if (p != null)
                {
                    User searchUser = p as User;
                    if (GlobalData.ins.currentUser.friendId != null && GlobalData.ins.currentUser.friendId.Contains(searchUser.id))
                    {
                        vm.CurrentRoom = await ConversationService.ins.GetRoomWithMyFriend(searchUser.id);
                    }
                    else
                    {
                        vm.CurrentRoom = await ConversationService.ins.CreateConversation(new List<String>() { GlobalData.ins.currentUser.id, searchUser.id });
                        GlobalData.ins.currentUser = await UserService.ins.GetUserById(GlobalData.ins.currentUser.id);
                    }

                    await Application.Current.MainPage.Navigation.PushAsync(new ChatScreen());
                }
            });
        }
    }
}
