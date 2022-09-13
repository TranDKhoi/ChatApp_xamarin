﻿using ChatApp_xamarin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace ChatApp_xamarin.ViewModels.Chat
{
    public class ConversationViewModel : BaseViewModel
    {

        private List<User> _users;
        public List<User> users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

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

        public ConversationViewModel()
        {
            users = new List<User>();
            tests = new List<Test>();
            for(int i=0; i < 10; i++)
            {
                Test test = new Test
                {
                    name = "Kiều Bá Dương",
                    message = "Chào em, anh đứng đây từ chiều!Chào em, anh đứng đây từ chiều!",
                    createdAt = DateTime.Now.ToShortTimeString(),
                    avatar = ImageSource.FromUri(new Uri("https://play-lh.googleusercontent.com/03URhAXU-IrK5PB-DiN6lyLGITlp-6xTizXkW5l98AUvpzOxQej6ss_zM4f8zxN0ofEf"))
                };
                tests.Add(test);
                User user = new User
                {
                    name = "Kiều Bá Dương",
                    imgSource = ImageSource.FromUri(new Uri("https://play-lh.googleusercontent.com/03URhAXU-IrK5PB-DiN6lyLGITlp-6xTizXkW5l98AUvpzOxQej6ss_zM4f8zxN0ofEf")),
                    

                };
                users.Add(user);
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
