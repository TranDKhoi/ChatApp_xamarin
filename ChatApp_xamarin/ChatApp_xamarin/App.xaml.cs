using Acr.UserDialogs;
using ChatApp_xamarin.Models;
using ChatApp_xamarin.Services;
using ChatApp_xamarin.Utils;
using ChatApp_xamarin.Views;
using ChatApp_xamarin.Views.Authentication.LoginScreen;
using ChatApp_xamarin.Views.Authentication.SignUpScreen;
using ChatApp_xamarin.Views.BottomBarCustom;
using ChatApp_xamarin.Views.Chat;
using Plugin.Multilingual;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ChatApp_xamarin
{
    public partial class Application : Xamarin.Forms.Application
    {
        public Application()
        {
            InitializeComponent();
            //get language
            var isVN = Preferences.Get("isVN", false);
            if (isVN)
                CrossMultilingual.Current.CurrentCultureInfo = new System.Globalization.CultureInfo("vi");
            else
                CrossMultilingual.Current.CurrentCultureInfo = new System.Globalization.CultureInfo("en");

            //get theme data
            var isDark = Preferences.Get("isDark", false);
            if (isDark)
                Application.Current.UserAppTheme = OSAppTheme.Dark;
            else
                Application.Current.UserAppTheme = OSAppTheme.Light;

            //get silent mode
            var isSilent = Preferences.Get("isSilent", false);
            GlobalData.ins.isSilentMode = isSilent;


            //get user login state
            var id = Preferences.Get("currentUser", null);

            if (id != null)
                MainPage = new NavigationPage(new BottomBarCustom());
            else
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
