using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp_xamarin.Models
{
    public class Room
    {
        public string id { get; set; }
        public List<string> isSeen { get; set; }
        public List<string> memberId { get; set; }
        public List<User> member { get; set; }
        public Message lastMessage { get; set; }
        public string roomName { get; set; }
        public List<string> isMuted { get; set; }
    }
}
