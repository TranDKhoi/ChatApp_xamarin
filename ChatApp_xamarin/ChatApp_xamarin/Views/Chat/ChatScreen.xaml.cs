using ChatApp_xamarin.ViewModels.Chat;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChatApp_xamarin.Views.Chat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatScreen : ContentPage
    {
        public ChatScreen()
        {
            InitializeComponent();

        }
        protected override void OnAppearing()
        {
            var viewModel = (ChatViewModel)this.BindingContext;
            viewModel.InitCM.Execute(MessageList);
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            var viewModel = (ChatViewModel)this.BindingContext;
            viewModel.messageListener.Dispose();
        }
    }
}