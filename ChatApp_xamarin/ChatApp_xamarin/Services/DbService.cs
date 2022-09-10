using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp_xamarin.Services
{
    internal class DbService
    {
        private static DbService _ins;
        public static DbService ins
        {
            get
            {
                if (_ins == null)
                    _ins = new DbService();
                return _ins;
            }
            set { _ins = value; }
        }

        private FirebaseClient _client;
        public FirebaseClient Client
        {
            get { return _client; }
            private set { _client = value; }
        }

        private const string _baseUrl = "https://chatapp-xamarin-default-rtdb.asia-southeast1.firebasedatabase.app/";

        public DbService()
        {
            Client = new FirebaseClient(_baseUrl);
        }
    }
}
