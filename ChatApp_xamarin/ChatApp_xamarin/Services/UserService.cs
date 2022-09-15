using ChatApp_xamarin.Models;
using ChatApp_xamarin.Resources;
using ChatApp_xamarin.Utils;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            User newU = new User
            {
                avatar = url,
            };
            await _client.Child($"users/{GlobalData.ins.currentUser.id}").PatchAsync(JsonConvert.SerializeObject(newU)); ;
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
            try
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
            catch (Exception)
            {
                return null;
            }
        }
    }
}
