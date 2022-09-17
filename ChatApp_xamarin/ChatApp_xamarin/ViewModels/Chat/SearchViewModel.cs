using ChatApp_xamarin.ViewModels.Friends;
using ChatApp_xamarin.Views.Chat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace ChatApp_xamarin.ViewModels.Chat
{
    public class SearchViewModel : BaseViewModel
    {
        private string _searchName;
        public string searchName
        {
            get { return _searchName; }
            set
            {
                _searchName = value;
                OnPropertyChanged();
            }
        }
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
        public SearchViewModel()
        {
            tests = new ObservableCollection<Test>();
            tests.Add(new Test
            {
                name = "Kiều Bá Dương",
                avatar = ImageSource.FromUri(new Uri("https://play-lh.googleusercontent.com/03URhAXU-IrK5PB-DiN6lyLGITlp-6xTizXkW5l98AUvpzOxQej6ss_zM4f8zxN0ofEf"))
            });
            tests.Add(new Test
            {
                name = "Trần Đình Khôi",
                avatar = ImageSource.FromUri(new Uri("https://play-lh.googleusercontent.com/03URhAXU-IrK5PB-DiN6lyLGITlp-6xTizXkW5l98AUvpzOxQej6ss_zM4f8zxN0ofEf"))
            });
            tests.Add(new Test
            {
                name = "Lê Hải Phong",
                avatar = ImageSource.FromUri(new Uri("https://play-lh.googleusercontent.com/03URhAXU-IrK5PB-DiN6lyLGITlp-6xTizXkW5l98AUvpzOxQej6ss_zM4f8zxN0ofEf"))
            });
            tests.Add(new Test
            {
                name = "Đỗ Thành Đạt",
                avatar = ImageSource.FromUri(new Uri("https://play-lh.googleusercontent.com/03URhAXU-IrK5PB-DiN6lyLGITlp-6xTizXkW5l98AUvpzOxQej6ss_zM4f8zxN0ofEf"))
            });
        }
    }
}
