﻿using Acr.UserDialogs;
using ChatApp_xamarin.Resources;
using ChatApp_xamarin.Views.Authentication.ForgotPass;

using System.Windows.Input;
using Xamarin.Forms;
using ChatApp_xamarin.Views.Authentication.SignUpScreen;
using ChatApp_xamarin.Services;
using ChatApp_xamarin.Models;
using ChatApp_xamarin.Utils;

namespace ChatApp_xamarin.ViewModels.Authentication
{
    public class LoginViewModel : BaseViewModel
    {
        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value; OnPropertyChanged();
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(); }
        }


        public ICommand LoginClickCM { get; set; }
        public ICommand ForgotPassClickCM { get; set; }
        public ICommand SignUpClickCM { get; set; }


        public LoginViewModel()
        {

            LoginClickCM = new Command(async (p) =>
            {
                if (isValidateData())
                {
                    if (!Validation.IsValidEmail(Email))
                    {
                        UserDialogs.Instance.Toast(AppResources.wrongemailorpassword);
                        return;
                    }
                    UserDialogs.Instance.ShowLoading();
                    (string mess, User user) = await AuthService.ins.Login(Email.Trim(), Password.Trim());
                    UserDialogs.Instance.HideLoading();

                    if (user == null)
                    {
                        UserDialogs.Instance.Toast(mess);
                        return;
                    }

                    //NAVIGATION TO HOME HERE

                }
                else
                {
                    UserDialogs.Instance.Toast(AppResources.pleaseenteryouremailorpassword);
                }
            });

            ForgotPassClickCM = new Command(async () =>
            {
                await App.Current.MainPage.Navigation.PushAsync(new ForgotPassScreen());
            });

            SignUpClickCM = new Command(async (p) =>
            {
                await App.Current.MainPage.Navigation.PushAsync(new SignUpScreen());
            });
        }


        bool isValidateData()
        {
            return !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
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