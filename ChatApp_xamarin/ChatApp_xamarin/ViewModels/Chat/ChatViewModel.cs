using ChatApp_xamarin.Models;
using ChatApp_xamarin.Services;
using ChatApp_xamarin.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ChatApp_xamarin.ViewModels.Chat
{
    public class ChatViewModel : BaseViewModel
    {
        public ICommand InitCM { get; set; }
        public ICommand BackCM { get; set; }
        public ICommand SendMessageCM { get; set; }
        public ICommand PickPhotoCM { get; set; }
        public ICommand SubscribeMessageChange { get; set; }

        private String _currentMessage;
        public String currentMessage
        {
            get { return _currentMessage; }
            set { _currentMessage = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Message> listMessage;
        public ObservableCollection<Message> ListMessage
        {
            get { return listMessage; }
            set { listMessage = value; OnPropertyChanged(); }
        }

        private Room currentRoom;
        public Room CurrentRoom
        {
            get { return currentRoom; }
            set { currentRoom = value; OnPropertyChanged(); }
        }

        private IDisposable messageListener;


        public ChatViewModel()
        {
            InitCM = new Command(async () =>
            {
                ListMessage = new ObservableCollection<Message>();
                var res = await MessageService.ins.GetRoomMessages(CurrentRoom.id);
                if (res != null)
                    ListMessage = new ObservableCollection<Message>(res.OrderBy(i => i.createdAt).ToList<Message>());
            });
            BackCM = new Command(async () =>
            {
                CurrentRoom = null;
                ListMessage = null;
                messageListener.Dispose();
                await Application.Current.MainPage.Navigation.PopAsync();
            });
            SendMessageCM = new Command(async () =>
            {
                try
                {
                    if (currentMessage != null)
                    {
                        await MessageService.ins.SendMessage(currentMessage.Trim(), CurrentRoom.id);
                        currentMessage = null;
                    }
                }
                catch (Exception e)
                {
                    throw (e);
                }
            });
            PickPhotoCM = new Command(() =>
            {
                try
                {
                }
                catch (Exception e)
                {
                    throw (e);
                }
            });
            SubscribeMessageChange = new Command(() =>
            {
                var subscribeer = MessageService.ins.SubscriptionToMessageChange(CurrentRoom.id);
                messageListener = subscribeer.Subscribe(item =>
                {
                    AddMoreMessage(item.Object);
                });
            });
        }

        private async void AddMoreMessage(Message mess)
        {
            foreach (var item in ListMessage)
            {
                if (item.createdAt == mess.createdAt) return;
            }

            mess.sender = await UserService.ins.GetUserById(mess.senderId);
            ListMessage.Add(mess);
        }
    }
}
