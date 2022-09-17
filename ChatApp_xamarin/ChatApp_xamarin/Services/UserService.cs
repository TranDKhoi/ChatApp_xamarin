using ChatApp_xamarin.Models;
using ChatApp_xamarin.Resources;
using ChatApp_xamarin.Utils;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp_xamarin.Services
{
    public class UserService
    {
        static private UserService _ins;
        static public UserService ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new UserService();
                }
                return _ins;
            }
            private set => _ins = value;
        }

        private FirebaseClient _client;


        UserService()
        {
            _client = DbService.ins.Client;
        }

        public async void UpdateUserAvatar(string url)
        {
            var upUser = GlobalData.ins.currentUser;
            upUser.avatar = url;
            await _client.Child($"users/{GlobalData.ins.currentUser.id}").PatchAsync(JsonConvert.SerializeObject(upUser));
        }

        public async Task<(String, User)> GetUserByEmail(string email)
        {
            try
            {
                var listUser = await _client.Child("users").OnceAsync<User>();

                if (listUser.Count == 0)
                {
                    return (AppResources.emaildoesnotexist, null);
                }
                else
                {
                    var doc = listUser.Where(x => x.Object.email == email).FirstOrDefault();
                    if (doc == null)
                    {
                        return (AppResources.emaildoesnotexist, null);
                    }

                    var trueUser = doc.Object;

                    trueUser.id = doc.Key;
                    return (null, trueUser);

                }
            }
            catch (Exception e)
            {
                return (e.Message, null);
            }
        }

        public async Task<User> GetUserById(string id)
        {
            var user = await _client.Child($"users/{id}").OnceSingleAsync<User>();

            if (user == null)
            {
                return null;
            }
            else
            {
                user.id = id;
                return user;
            }
        }

        public async Task UpdateRoomKey(List<string> memberId, string roomId)
        {
            foreach (var item in memberId)
            {
                var user = await GetUserById(item);
                if (user != null)
                {
                    if (user.roomKey is null)
                    {
                        user.roomKey = new List<string>();
                        user.roomKey.Add(roomId);
                        await _client.Child($"users/{user.id}").PutAsync(JsonConvert.SerializeObject(user));
                    }
                    if (!user.roomKey.Contains(roomId))
                    {
                        user.roomKey.Add(roomId);
                        await _client.Child($"users/{user.id}").PutAsync(JsonConvert.SerializeObject(user));
                    }
                }

            }
        }

        public async Task UpdateFriend(string friendID)
        {
            if (GlobalData.ins.currentUser.friendId is null)
            {
                GlobalData.ins.currentUser.friendId = new List<string>();
                GlobalData.ins.currentUser.friendId.Add(friendID);
            }
            if (!GlobalData.ins.currentUser.friendId.Contains(friendID))
            {
                GlobalData.ins.currentUser.friendId.Add(friendID);
            }

            await _client.Child($"users/{GlobalData.ins.currentUser.id}").PatchAsync(JsonConvert.SerializeObject(GlobalData.ins.currentUser));

            var friend = await _client.Child($"users/{friendID}").OnceSingleAsync<User>();

            if (friend.friendId is null)
            {
                friend.friendId = new List<string>();
                friend.friendId.Add(GlobalData.ins.currentUser.id);
            }
            if (!friend.friendId.Contains(GlobalData.ins.currentUser.id))
            {
                friend.friendId.Add(GlobalData.ins.currentUser.id);
            }

            await _client.Child($"users/{friend.id}").PatchAsync(JsonConvert.SerializeObject(friend));
        }

        public async Task<List<User>> GetUserByName(string userName)
        {

            var res = await _client.Child("users").OnceAsync<User>();

            List<User> listMatchedName = null;

            if (res.Count != 0)
            {
                listMatchedName = new List<User>();
                listMatchedName = res.Select(i => i.Object).Where(item => item.name == userName).ToList();
            }

            return listMatchedName;
        }

        public async Task<List<User>> GetOnlineFriend(List<string> friendId)
        {
            var listUser = await _client.Child("users").OnceAsync<User>();

            List<User> listOnline = new List<User>();

            foreach (var id in friendId)
            {
                listOnline.Add(listUser.Select(u => u.Object).Where(i => i.id == id).First());
            }

            return listOnline;

        }
    }
}
