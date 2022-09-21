namespace ChatApp_xamarin.Models
{
    public class Message
    {
        public string id { get; set; }
        public string senderId { get; set; }
        public User sender { get; set; }
        public string createdAt { get; set; }
        public string message { get; set; }
        public string image { get; set; }
    }
}
