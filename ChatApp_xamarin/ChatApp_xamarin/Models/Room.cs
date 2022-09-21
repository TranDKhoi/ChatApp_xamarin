using System.Collections.Generic;

namespace ChatApp_xamarin.Models
{
    public class Room
    {
        public string id { get; set; }
        public List<string> isSeen { get; set; }
        public List<string> memberId { get; set; }
        public List<User> member { get; set; }
        public Message lastMessage { get; set; }
        public string lastUpdate { get; set; }
        public string roomName { get; set; }
        public List<string> isMuted { get; set; }
        public string avatar { get; set; }
    }
}
