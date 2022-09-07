using ChatApp_xamarin.Views.Authentication.LoginScreen;
using Plugin.Multilingual;
using Xamarin.Forms;

namespace ChatApp_xamarin
{
    public partial class App : Application
    {
        public App()
        {
          
            InitializeComponent();

            CrossMultilingual.Current.CurrentCultureInfo = new System.Globalization.CultureInfo("vi");

            MainPage = new LoginScreen();
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
