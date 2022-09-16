using ChatApp_xamarin.Models;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using ChatApp_xamarin.Utils;
using Firebase.Database.Streaming;
using System.Reactive.Linq;
using Firebase.Database.Query;

namespace ChatApp_xamarin.Services
{
    public class ConversationService
    {
        static private ConversationService _ins;
        static public ConversationService ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new ConversationService();
                }
                return _ins;
            }
            private set => _ins = value;
        }

        private FirebaseClient _client;
        ConversationService()
        {
            _client = DbService.ins.Client;
        }

        public async Task<List<Room>> GetAllConversation(List<string> roomKey)
        {
            ///fetch all room
            List<Room> listCv = new List<Room>();
            if (roomKey == null) return listCv;

            foreach (var item in roomKey)
            {
                var room = await _client.Child($"rooms/{item}").OnceSingleAsync<Room>();
                if (room != null)
                    listCv.Add(room);
            }

            //fetch user data for room
            for (int i = 0; i < listCv.Count; i++)
            {
                listCv[i].member = new List<User>();
                foreach (var memberId in listCv[i].memberId)
                {
                    var u = await UserService.ins.GetUserById(memberId);
                    listCv[i].member.Add(u);
                }
                //fetch last message of this room
                listCv[i].lastMessage = await MessageService.ins.GetLastMessage(listCv[i].id);
            }


            //return listCv.OrderByDescending(item => item.lastMessage?.createdAt).ToList();
            return listCv;
        }

        public async Task<string> CreateConversation(List<string> memberID)
        {
            var uniqueID = Guid.NewGuid().ToString();
            Room newRoom = new Room
            {
                id = uniqueID,
                memberId = new List<string>(memberID),
            };
            var json = JsonConvert.SerializeObject(newRoom);
            await _client.Child($"rooms/{uniqueID}").PutAsync(json);

            _ = UserService.ins.UpdateRoomKey(memberID, uniqueID);

            return uniqueID;
        }

        public async void SeenLastMessage(string roomId)
        {
            var room = await _client.Child($"rooms/{roomId}").OnceSingleAsync<Room>();

            if (room.isSeen is null)
            {
                room.isSeen = new List<string>();
                room.isSeen.Add(GlobalData.ins.currentUser.id);
            }
            if (!room.isSeen.Contains(GlobalData.ins.currentUser.id))
            {
                room.isSeen.Add(GlobalData.ins.currentUser.id);
            }

            await _client.Child($"rooms/{roomId}").PatchAsync(JsonConvert.SerializeObject(room));
        }

        public IObservable<FirebaseEvent<Room>> SubscriptionToConversationChange()
        {
            var child = _client.Child("rooms");
            var observable = child.AsObservable<Room>();
            return observable
                .Where(f => !string.IsNullOrEmpty(f.Key));
        }

        public async Task UpdateLastMessage(string roomId, Message lassmess)
        {
            var room = await _client.Child($"rooms/{roomId}").OnceSingleAsync<Room>();

            room.lastMessage = lassmess;
            await _client.Child($"rooms/{roomId}").PatchAsync(JsonConvert.SerializeObject(room));
        }

    }
}
