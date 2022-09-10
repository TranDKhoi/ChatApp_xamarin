using ChatApp_xamarin.Models;
using Firebase.Database;
using Firebase.Database.Streaming;
using System;
using System.Reactive.Linq;

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

        public IObservable<FirebaseEvent<Message>> SubscriptionToMessageChange()
        {
            var child = _client.Child("users");
            var observable = child.AsObservable<Message>();
            return observable
                .Where(f => !string.IsNullOrEmpty(f.Key));
        }

    }
}
