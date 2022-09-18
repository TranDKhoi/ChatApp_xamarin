using ChatApp_xamarin.Models;
using ChatApp_xamarin.Resources;
using ChatApp_xamarin.Utils;
using Firebase.Database;
using Firebase.Database.Streaming;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace ChatApp_xamarin.Services
{
    public class MessageService
    {
        private static MessageService _ins;
        public static MessageService ins
        {
            get
            {
                if (_ins == null)
                    _ins = new MessageService();
                return _ins;
            }
            set { _ins = value; }
        }

        private FirebaseClient _client;

        MessageService()
        {
            _client = DbService.ins.Client;
        }

        public IObservable<FirebaseEvent<Message>> SubscriptionToMessageChange(string roomId)
        {
            var child = _client.Child($"messages/{roomId}");
            var observable = child.AsObservable<Message>();
            return observable
                .Where(f => !string.IsNullOrEmpty(f.Key));
        }

        public async Task<Message> GetLastMessage(string roomId)
        {
            var res = await _client.Child($"messages/{roomId}").OnceAsync<Message>();
            var lastMess = res.OrderByDescending(m => m.Object.createdAt).FirstOrDefault();
            if (lastMess != null)
                return lastMess.Object;
            return null;
        }

        public async Task SendMessage(string mess, string roomID)
        {
            Message newMess = new Message
            {
                createdAt = DateTime.Now.ToString(),
                message = mess,
                senderId = GlobalData.ins.currentUser.id,
            };

            var json = JsonConvert.SerializeObject(newMess);
            var res = await _client.Child($"messages/{roomID}").PostAsync(json);

            await ConversationService.ins.UpdateLastMessage(roomID, newMess);
        }

        public async Task SendImage(string roomId, MediaFile file)
        {
            var link = await StorageService.ins.UploadMessageImage(file);

            Message newMess = new Message
            {
                createdAt = DateTime.Now.ToString(),
                senderId = GlobalData.ins.currentUser.id,
                image = link,
            };

            var json = JsonConvert.SerializeObject(newMess);
            var res = await _client.Child($"messages/{roomId}").PostAsync(json);

            await ConversationService.ins.UpdateLastMessage(roomId, newMess);
        }

        public async Task<List<Message>> GetRoomMessages(string roomId)
        {
            List<Message> res = new List<Message>();

            var listRes = await _client.Child($"messages/{roomId}").OnceAsync<Message>();

            if (listRes.Count == 0) return null;

            res = listRes.Select(item => item.Object).OrderBy(i => i.createdAt).ToList();

            var listIdQueried = new List<string>();
            foreach (var mess in res)
            {
                if (listIdQueried.Contains(mess.senderId)) continue;
                mess.sender = await UserService.ins.GetUserById(mess.senderId);
            }
            return res;
        }
    }
}
