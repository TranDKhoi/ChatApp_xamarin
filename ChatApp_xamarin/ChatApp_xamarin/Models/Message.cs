using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp_xamarin.Models
{
    public class Message
    {
        public string id { get; set; }
        public string senderId { get; set; }
        public User sender { get; set; }
        public string createdAt { get; set; }
        public string message { get; set; }
        public List<string> images { get; set; }
    }
}
