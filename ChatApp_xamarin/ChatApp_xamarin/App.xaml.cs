using Acr.UserDialogs;
using ChatApp_xamarin.Views.Authentication.LoginScreen;
using ChatApp_xamarin.Views.Authentication.SignUpScreen;
using ChatApp_xamarin.Views.BottomBarCustom;
using Plugin.Multilingual;
using Xamarin.Forms;

namespace ChatApp_xamarin
{
    public partial class App : Application
    {
        public App()
        {

            InitializeComponent();

            CrossMultilingual.Current.CurrentCultureInfo = new System.Globalization.CultureInfo("en");
            MainPage = new NavigationPage(new LoginScreen());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
