using ChatApp_xamarin.Models;
using ChatApp_xamarin.Services;
using ChatApp_xamarin.Utils;
using ChatApp_xamarin.Views.Authentication.ForgotPass;
using ChatApp_xamarin.Views.Chat;
using ChatApp_xamarin.Views.Group;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace ChatApp_xamarin.ViewModels.Chat
{
    public class ConversationViewModel : BaseViewModel
    {
        public ICommand OpenChatScreenVM { get; set; }
        public ICommand OpenSearchScreenVM { get; set; }
        public ICommand GetAllConversation { get; set; }

        private ObservableCollection<Room> listConversation;
        public ObservableCollection<Room> ListConversation
        {
            get { return listConversation; }
            set { listConversation = value; OnPropertyChanged(); }
        }


        public ConversationViewModel()
        {
            OpenChatScreenVM = new Command(async (p) =>
            {
                Room selectedRoom = p as Room;
                ChatScreen cs = new ChatScreen();
                var vm = (ChatViewModel)cs.BindingContext;
                vm.CurrentRoom = selectedRoom;
                await Application.Current.MainPage.Navigation.PushAsync(new ChatScreen());
            });

            GetAllConversation = new Command(async () =>
            {
                ListConversation = new ObservableCollection<Room>(await ConversationService.ins.GetAllConversation(GlobalData.ins.currentUser.roomKey));
            });

            OpenSearchScreenVM = new Command(async (p) =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new SearchScreen());
            });
        }
    }
}
