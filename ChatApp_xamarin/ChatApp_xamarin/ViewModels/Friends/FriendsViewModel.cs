using ChatApp_xamarin.Models;
using ChatApp_xamarin.ViewModels.Chat;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ChatApp_xamarin.ViewModels.Friends
{
    public class FriendsViewModel : BaseViewModel
    {
        private List<Test> _tests;
        public List<Test> tests
        {
            get { return _tests; }
            set
            {
                _tests = value;
                OnPropertyChanged();
            }
        }

        public FriendsViewModel()
        {
            tests = new List<Test>();
            for (int i = 0; i < 10; i++)
            {
                Test test = new Test
                {
                    name = "Kiều Bá Dương",
                    message = "Chào em, anh đứng đây từ chiều!Chào em, anh đứng đây từ chiều!",
                    createdAt = DateTime.Now.ToShortTimeString(),
                    avatar = ImageSource.FromUri(new Uri("https://play-lh.googleusercontent.com/03URhAXU-IrK5PB-DiN6lyLGITlp-6xTizXkW5l98AUvpzOxQej6ss_zM4f8zxN0ofEf"))
                };
                tests.Add(test);
            }
        }
    }
    public class Test
    {
        public ImageSource avatar { get; set; }
        public string name { get; set; }
        public string message { get; set; }
        public string createdAt { get; set; }
    }
}
