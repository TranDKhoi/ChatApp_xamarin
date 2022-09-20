﻿using ChatApp_xamarin.Models;
using ChatApp_xamarin.Services;
using ChatApp_xamarin.Views.Group;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace ChatApp_xamarin.ViewModels.Chat
{
    public class ChatViewModel : BaseViewModel
    {
        public ICommand InitCM { get; set; }
        public ICommand BackCM { get; set; }
        public ICommand SendMessageCM { get; set; }
        public ICommand PickPhotoCM { get; set; }
        public ICommand TakePhotoCM { get; set; }
        public ICommand SubscribeMessageChange { get; set; }
        public ICommand OpenGroupScreenVM { get; set; }
        public ICommand PickAvatarForGroupCM { get; set; }

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

        public IDisposable messageListener;
        public CollectionView collectionView { get; set; }


        public ChatViewModel()
        {
            InitCM = new Command((p) =>
            {
                currentMessage = "";
                collectionView = p as CollectionView;
                ListMessage = new ObservableCollection<Message>();
                SubscribeMessageChange.Execute(null);
                if (ListMessage.Count == 0) return;
                collectionView.ScrollTo(ListMessage.Last(), null, ScrollToPosition.End, true);
            });
            BackCM = new Command(async () =>
            {
                CurrentRoom = null;
                ListMessage = null;
                await Application.Current.MainPage.Navigation.PopAsync();
                var converVM = Application.Current.Resources["ConversationVM"] as ConversationViewModel;

                converVM.GetAllConversation.Execute(null);
            });
            SendMessageCM = new Command(async (p) =>
            {
                try
                {
                    if (currentMessage != null)
                    {
                        CollectionView collectionView = p as CollectionView;
                        await MessageService.ins.SendMessage(currentMessage.Trim(), CurrentRoom.id);
                        currentMessage = null;
                        if (ListMessage.Count == 0) return;
                        collectionView.ScrollTo(ListMessage.Last(), null, ScrollToPosition.End, true);
                        return;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            });
            PickPhotoCM = new Command(async () =>
            {
                try
                {
                    //pick image
                    await CrossMedia.Current.Initialize();
                    var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    {
                        PhotoSize = PhotoSize.Medium
                    });

                    if (file == null)
                        return;

                    await MessageService.ins.SendImage(CurrentRoom.id, file);
                    InitCM.Execute(null);
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
            OpenGroupScreenVM = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new GroupScreen());
            });
            PickAvatarForGroupCM = new Command(async () =>
            {
                if (CurrentRoom.memberId.Count == 2) return;

                //pick image
                await CrossMedia.Current.Initialize();
                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });

                if (file == null)
                    return;

                var link = await ConversationService.ins.UpdateRoomAvatar(CurrentRoom.id, file);
                CurrentRoom.avatar = link;
            });
            TakePhotoCM = new Command(async () =>
            {
                MediaFile photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    DefaultCamera = CameraDevice.Rear,
                    Name = null,
                    CompressionQuality = 100
                });
                if (photo == null)
                    return;

                await MessageService.ins.SendImage(CurrentRoom.id, photo);
                InitCM.Execute(null);
            });
        }

        private void AddMoreMessage(Message mess)
        {
            try
            {
                mess.sender = CurrentRoom.member.Where(i => i.id == mess.senderId).First();
                ListMessage.Add(mess);
                collectionView.ScrollTo(ListMessage.Last(), null, ScrollToPosition.End, true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
