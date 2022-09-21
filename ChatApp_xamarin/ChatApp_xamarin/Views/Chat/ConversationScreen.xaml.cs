using ChatApp_xamarin.Services;
using ChatApp_xamarin.Utils;
using ChatApp_xamarin.ViewModels;
using ChatApp_xamarin.ViewModels.Chat;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChatApp_xamarin.Views.Chat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConversationScreen : ContentPage
    {
        public ConversationScreen()
        {
            InitializeComponent();
        }

        private void ListConversation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListConversation.SelectedItem == null)
            {
                return;
            }
            var viewModel = (ConversationViewModel)this.BindingContext;
            viewModel.OpenChatScreenVM.Execute(ListConversation.SelectedItem);
            ListConversation.SelectedItem = null;
        }

        protected override async void OnAppearing()
        {
            if (GlobalData.ins.currentUser == null)
            {
                var id = Preferences.Get("currentUser", null);
                GlobalData.ins.currentUser = await UserService.ins.GetUserById(id);
            }
            var bottomVM = Application.Current.Resources["BottomVM"] as BottomBarViewModel;
            bottomVM.SubscribeToConversationCM.Execute(null);
        }
    }
}