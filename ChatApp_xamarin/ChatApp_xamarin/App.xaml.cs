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
            var isvn = preferences.get("isvn", false);
            if (isvn)
                crossmultilingual.current.currentcultureinfo = new system.globalization.cultureinfo("vi");
            else
                crossmultilingual.current.currentcultureinfo = new system.globalization.cultureinfo("en");

            //get theme data
            var isdark = preferences.get("isdark", false);
            if (isdark)
                application.current.userapptheme = osapptheme.dark;
            else
                application.current.userapptheme = osapptheme.light;

            //get silent mode
            var issilent = preferences.get("issilent", false);
            globaldata.ins.issilentmode = issilent;


            //get user login state
            var id = preferences.get("currentuser", null);

            if (id != null)
                mainpage = new navigationpage(new bottombarcustom());
            else
                mainpage = new navigationpage(new loginscreen());
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
