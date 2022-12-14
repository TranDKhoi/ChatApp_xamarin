using ChatApp_xamarin.Services;
using ChatApp_xamarin.Utils;
using ChatApp_xamarin.Views.Authentication.LoginScreen;
using ChatApp_xamarin.Views.BottomBarCustom;
using Plugin.Multilingual;
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

        protected override async void OnStart()
        {
            var id = Preferences.Get("currentUser", null);

            if (id != null)
                await AuthService.ins.Online(id);
        }

        protected override void OnSleep()
        {
            var id = Preferences.Get("currentUser", null);

            if (id != null)
                AuthService.ins.Logout(id);
        }

        protected override async void OnResume()
        {
            var id = Preferences.Get("currentUser", null);

            if (id != null)
                await AuthService.ins.Online(id);
        }
    }
}
