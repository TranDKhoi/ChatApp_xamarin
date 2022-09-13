using ChatApp_xamarin.Models;
using ChatApp_xamarin.Resources;
using ChatApp_xamarin.Utils;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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

        public async void UpDateUserAvatar(string url)
        {
            User newU = new User
            {
                avatar = url,
            };
            await _client.Child($"users/{GlobalData.ins.currentUser.id}").PatchAsync(JsonConvert.SerializeObject(newU)); ;
        }
    }
}
