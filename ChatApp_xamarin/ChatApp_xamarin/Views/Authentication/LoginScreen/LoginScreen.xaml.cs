using ChatApp_xamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChatApp_xamarin.Views.Authentication.LoginScreen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginScreen : ContentPage
    {
        public LoginScreen()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var bottomVM = Application.Current.Resources["BottomVM"] as BottomBarViewModel;
            bottomVM.UnSubscribeToConversationCM.Execute(null);
        }
    }
}