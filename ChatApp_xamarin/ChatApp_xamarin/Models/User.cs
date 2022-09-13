using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ChatApp_xamarin.Models
{
    public class User
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public string emailVerified { get; set; }
        public string avatar { get; set; }
        public ImageSource imgSource { get; set; }
    }
}
