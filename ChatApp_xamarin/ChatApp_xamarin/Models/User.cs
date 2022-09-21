using System.Collections.Generic;

namespace ChatApp_xamarin.Models
{
    public class User
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string avatar { get; set; }
        public List<string> roomKey { get; set; }
        public List<string> friendId { get; set; }
        public bool isOnline { get; set; }
    }
}
