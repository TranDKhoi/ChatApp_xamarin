using ChatApp_xamarin.Models;
using ChatApp_xamarin.ViewModels.Chat;
using ChatApp_xamarin.Views.Chat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ChatApp_xamarin.ViewModels.Friends
{
    public class FriendsViewModel : BaseViewModel
    {
        public ICommand OpenChatScreenVM { get; set; }
        private ObservableCollection<Test> _tests;
        public ObservableCollection<Test> tests
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
            tests = new ObservableCollection<Test>();
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
            OpenChatScreenVM = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ChatScreen());
            });
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
