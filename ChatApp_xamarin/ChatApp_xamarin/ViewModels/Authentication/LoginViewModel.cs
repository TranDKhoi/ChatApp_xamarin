using ChatApp_xamarin.Views.Authentication.LoginScreen;
using Plugin.Multilingual;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;

namespace ChatApp_xamarin.ViewModels.Authentication
{
    public class LoginViewModel : BaseViewModel
    {
        private string test;
        public string Test
        {
            get { return test; }
            set
            {
                test = value;
                OnPropertyChanged();
            }
        }




        public ICommand tapCM { get; set; }


        public LoginViewModel()
        {
            tapCM = new Command(() =>
            {
                

            });
        }

    }
}














//CHANGE LANGUAGE
//CrossMultilingual.Current.CurrentCultureInfo = new System.Globalization.CultureInfo("en");
//App.Current.MainPage = new LoginScreen();


//CHANGE THEME
//if (App.Current.RequestedTheme == OSAppTheme.Light)
//{
//    App.Current.UserAppTheme = OSAppTheme.Dark;
//}
//else
//{
//    App.Current.UserAppTheme = OSAppTheme.Light;
//}