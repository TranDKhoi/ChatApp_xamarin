using ChatApp_xamarin.Models;
using ChatApp_xamarin.Services;
using ChatApp_xamarin.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using ChatApp_xamarin.ViewModels.Chat;
using Plugin.Media.Abstractions;
using Acr.UserDialogs;
using ChatApp_xamarin.Resources;

namespace ChatApp_xamarin.ViewModels.Group
{
    public class GroupViewModel : BaseViewModel
    {
        private ObservableCollection<User> _users;
        public ObservableCollection<User> users
        {
            get { return _users; }
            set { _users = value; OnPropertyChanged(); }
        }

        private ObservableCollection<User> selectedMemberqueue;
        public ObservableCollection<User> SelectedMemberQueue
        {
            get { return selectedMemberqueue; }
            set { selectedMemberqueue = value; OnPropertyChanged(); }
        }

        private User currentSelect;
        public User CurrentSelect
        {
            get { return currentSelect; }
            set { currentSelect = value; OnPropertyChanged(); }
        }

        private string roomName;
        public string RoomName
        {
            get { return roomName; }
            set { roomName = value; OnPropertyChanged(); }
        }


        public ICommand BackCM { get; set; }
        public ICommand SearchCM { get; set; }
        public ICommand AddToMemberQueueCM { get; set; }
        public ICommand CreateGroupCM { get; set; }



        public GroupViewModel()
        {
            BackCM = new Command(async () =>
            {
                CurrentSelect = null;
                RoomName = "";
                await Application.Current.MainPage.Navigation.PopAsync();
            });
            SearchCM = new Command(async (p) =>
            {
                Entry s = p as Entry;

                if (!string.IsNullOrEmpty(s.Text))
                    users = new ObservableCollection<User>((await UserService.ins.GetUserByName(s.Text))
                        .Where(i => i.id != GlobalData.ins.currentUser.id));
                else
                    users = new ObservableCollection<User>(await UserService.ins.GetOnlineFriend(GlobalData.ins.currentUser.friendId));
            });
            AddToMemberQueueCM = new Command((p) =>
            {
                CurrentSelect = p as User;
            });
            CreateGroupCM = new Command(async () =>
            {
                var chatVM = Application.Current.Resources["ChatVM"] as ChatViewModel;

                if (chatVM.CurrentRoom.memberId.Contains(CurrentSelect.id))
                {
                    UserDialogs.Instance.Toast(AppResources.memberalreadyexisted);
                    return;
                }

                if (chatVM.CurrentRoom.memberId.Count == 2)
                {
                    if (!isValidData())
                    {
                        UserDialogs.Instance.Toast(AppResources.pleaseentergroupname);
                        return;
                    }

                    UserDialogs.Instance.ShowLoading();
                    List<string> memberId = new List<string>(chatVM.CurrentRoom.memberId);
                    memberId.Add(CurrentSelect.id);
                    await ConversationService.ins.CreateGroupConversation(memberId, RoomName);
                    UserDialogs.Instance.HideLoading();
                }
                else
                {
                    UserDialogs.Instance.ShowLoading();
                    await ConversationService.ins.AddMemberToGroup(chatVM.CurrentRoom.id, CurrentSelect.id);
                    UserDialogs.Instance.HideLoading();
                }
                BackCM.Execute(null);
            });
        }

        bool isValidData()
        {
            return !String.IsNullOrEmpty(RoomName);
        }
    }
}
