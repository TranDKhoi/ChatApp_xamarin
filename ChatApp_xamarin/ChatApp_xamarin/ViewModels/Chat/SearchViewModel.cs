using ChatApp_xamarin.Models;
using ChatApp_xamarin.Services;
using ChatApp_xamarin.Utils;
using ChatApp_xamarin.ViewModels.Friends;
using ChatApp_xamarin.Views.Chat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ChatApp_xamarin.ViewModels.Chat
{
    public class SearchViewModel : BaseViewModel
    {
        public ICommand BackCM { get; set; }
        public ICommand SearchGroupCM { get; set; }
        public ICommand OpenChatScreenCM { get; set; }


        private string _searchGroupName;
        public string searchGroupName
        {
            get { return _searchGroupName; }
            set
            {
                _searchGroupName = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Room> _rooms;
        public ObservableCollection<Room> rooms
        {
            get { return _rooms; }
            set
            {
                _rooms = value;
                OnPropertyChanged();
            }
        }
        public SearchViewModel()
        {

            BackCM = new Command(async () =>
            {
                searchGroupName = String.Empty;
                await Application.Current.MainPage.Navigation.PopAsync();
            });

            SearchGroupCM = new Command(async () =>
             {
                 if (rooms != null)
                 {
                     rooms.Clear();
                 }
                 if (!string.IsNullOrEmpty(searchGroupName))
                 {
                     List<Room> searchRoom = new List<Room>();
                     var allRoom = await ConversationService.ins.GetAllConversation(GlobalData.ins.currentUser.roomKey);
                     if (allRoom != null)
                     {
                         for (int i = 0; i < allRoom.Count; i++)
                         {
                             if (allRoom[i].roomName.ToLower().Contains(searchGroupName.ToLower()))
                             {
                                 searchRoom.Add(allRoom[i]);
                             }
                         }
                         rooms = new ObservableCollection<Room>(searchRoom);
                     }
                 }
             });

            OpenChatScreenCM = new Command(async (p) =>
            {
                Room selectedRoom = p as Room;
                ChatScreen cs = new ChatScreen();
                var vm = (ChatViewModel)cs.BindingContext;
                vm.CurrentRoom = selectedRoom;
                await Application.Current.MainPage.Navigation.PushAsync(new ChatScreen());
                searchGroupName = string.Empty;
            });
        }
    }
}
