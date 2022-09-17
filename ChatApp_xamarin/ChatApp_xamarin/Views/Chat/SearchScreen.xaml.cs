using ChatApp_xamarin.ViewModels.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChatApp_xamarin.Views.Chat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchScreen : ContentPage
    {
        public SearchScreen()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            searchEntry.Focus();
        }

        private void listFriends_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listFriends.ItemsSource == null)
            {
                return;
            }
            var viewModel = (SearchViewModel)this.BindingContext;
            //viewModel.OpenChatScreenVM.Execute(listFriends.SelectedItem);
            //ListConversation.SelectedItem = null;
        }
    }
}