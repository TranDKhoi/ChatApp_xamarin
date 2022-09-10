using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp_xamarin.ViewModels.Chat
{
    public class ConversationViewModel : BaseViewModel
    {
        private string search;
        public string Search
        {
            get { return search; }
            set { search = value; OnPropertyChanged(); }
        }
    }
}
