using ChatApp_xamarin.Models;
using ChatApp_xamarin.Utils;
using Firebase.Database;
using Firebase.Database.Query;
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
                if (item is null) continue;
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
                    if (u != null)
                        listCv[i].member.Add(u);
                }
            }

            //retrieve name for room
            foreach (var r in listCv)
            {
                if (r.roomName is null)
                {
                    var partnerName = r.member.Where(m => m.id != GlobalData.ins.currentUser.id).Select(m => m.name).First();
                    r.roomName = partnerName;
                }
            }

            return listCv.OrderByDescending(item => item.lastUpdate).ToList();
        }

        public async Task<Room> CreateConversation(List<string> memberID)
        {

            var partnerId = memberID.Where(u => u != GlobalData.ins.currentUser.id).First(); ;

            var partner = await UserService.ins.GetUserById(partnerId);

            var uniqueID = Guid.NewGuid().ToString();
            Room newRoom = new Room
            {
                id = uniqueID,
                memberId = new List<string>(memberID),
                lastUpdate = DateTime.Now.ToString(),
            };
            var json = JsonConvert.SerializeObject(newRoom);
            await _client.Child($"rooms/{uniqueID}").PutAsync(json);

            newRoom.roomName = partner.name;

            newRoom.member = new List<User>();
            foreach (var memberId in newRoom.memberId)
            {
                var u = await UserService.ins.GetUserById(memberId);
                newRoom.member.Add(u);
            }
            //fetch last message of this room
            newRoom.lastMessage = await MessageService.ins.GetLastMessage(newRoom.id);

            await UserService.ins.UpdateRoomKey(memberID, uniqueID);

            var friendId = memberID.Where(id => id != GlobalData.ins.currentUser.id).First();

            await UserService.ins.UpdateFriend(friendId);

            return newRoom;
        }

        public async Task<Room> CreateGroupConversation(List<string> memberID, string roomName)
        {
            var uniqueID = Guid.NewGuid().ToString();

            Room newGroup = new Room
            {
                id = uniqueID,
                memberId = new List<string>(memberID),
                roomName = roomName,
                lastUpdate = DateTime.Now.ToString(),
            };
            var json = JsonConvert.SerializeObject(newGroup);
            await _client.Child($"rooms/{uniqueID}").PutAsync(json);

            _ = UserService.ins.UpdateRoomKey(memberID, uniqueID);

            return newGroup;
        }

        public async Task AddMemberToGroup(string roomId, string memberId)
        {
            var room = await _client.Child($"rooms/{roomId}").OnceSingleAsync<Room>();

            room.memberId.Add(memberId);
            room.lastUpdate = DateTime.Now.ToString();

            _ = UserService.ins.UpdateRoomKey(new List<string> { memberId }, room.id);

            await _client.Child($"rooms/{roomId}").PatchAsync(JsonConvert.SerializeObject(room));
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
            room.lastUpdate = DateTime.Now.ToString();

            room.isSeen = new List<string>();
            room.isSeen.Add(GlobalData.ins.currentUser.id);

            await _client.Child($"rooms/{roomId}").PatchAsync(JsonConvert.SerializeObject(room));
        }

        public async Task UpdateRoomName(string roomId, string newName)
        {
            var group = await _client.Child($"rooms/{roomId}").OnceSingleAsync<Room>();
            group.roomName = newName;
            group.lastUpdate = DateTime.Now.ToString();
            await _client.Child($"rooms/{roomId}").PatchAsync<Room>(group);
        }

        public async Task<string> UpdateRoomAvatar(string roomId, MediaFile avt)
        {
            var room = await _client.Child($"rooms/{roomId}").OnceSingleAsync<Room>();

            var link = await StorageService.ins.UploadGroupImage(roomId, avt);

            room.avatar = link;

            await _client.Child($"rooms/{roomId}").PatchAsync(JsonConvert.SerializeObject(room));
            return link;
        }

        public async Task<Room> GetRoomWithMyFriend(string friendId)
        {
            var allRoom = await _client.Child("rooms").OnceAsync<Room>();

            if (allRoom == null) return null;

            var listRoomWith2Mem = allRoom.Select(r => r.Object).Where(r => r.memberId.Count == 2 && r.memberId.Contains(friendId));

            var res = listRoomWith2Mem.Where(r => r.memberId.Contains(GlobalData.ins.currentUser.id)).First();

            //fetch user data for room
            res.member = new List<User>();
            foreach (var memberId in res.memberId)
            {
                var u = await UserService.ins.GetUserById(memberId);
                if (u != null)
                    res.member.Add(u);
            }
            //fetch last message of this room
            res.lastMessage = await MessageService.ins.GetLastMessage(res.id);

            //get room name
            if (res.roomName is null)
            {
                var partnerName = res.member.Where(m => m.id != GlobalData.ins.currentUser.id).Select(m => m.name).First();
                res.roomName = partnerName;
            }

            return res;
        }
    }
}
