using Acr.UserDialogs;
using ChatApp_xamarin.Models;
using ChatApp_xamarin.Resources;
using ChatApp_xamarin.Services;
using ChatApp_xamarin.Utils;
using ChatApp_xamarin.Views.Authentication.LoginScreen;
using ChatApp_xamarin.Views.Authentication.SignUpScreen;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace ChatApp_xamarin.ViewModels.Authentication
{
    public class SignUpViewModel : BaseViewModel
    {

        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(); }
        }

        private string rePassword;
        public string RePassword
        {
            get { return rePassword; }
            set { rePassword = value; OnPropertyChanged(); }
        }

        private string displayName;
        public string Displayname
        {
            get { return displayName; }
            set { displayName = value; OnPropertyChanged(); }
        }


        private int verifycationCode;

        public ICommand PopClickCM { get; set; }
        public ICommand ConfirmClickCM { get; set; }
        public ICommand VerifyEmailClickCM { get; set; }
        public ICommand PopToSignUpClickCM { get; set; }

        public SignUpViewModel()
        {
            PopClickCM = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });
            ConfirmClickCM = new Command(async (p) =>
            {
                (string res, bool isS) = isValidateData();

                if (!isS)
                {
                    UserDialogs.Instance.Toast(res);
                    return;
                }
                else
                {
                    UserDialogs.Instance.ShowLoading();
                    verifycationCode = await AuthService.ins.SendVerifyCode(Email.Trim());
                    UserDialogs.Instance.HideLoading();
                    await Application.Current.MainPage.DisplayAlert(AppResources.alert, AppResources.checkyouremail, "OK");
                    await Application.Current.MainPage.Navigation.PushAsync(new VerifyEmailScreen());
                }
            });
            VerifyEmailClickCM = new Command(async (p) =>
            {
                Entry en = p as Entry;
                if (string.IsNullOrEmpty(en.Text)) return;

                if (int.Parse(en.Text.Trim()) == verifycationCode)
                {
                    UserDialogs.Instance.ShowLoading();

                    User u = new User
                    {
                        email = Email,
                        password = Password,
                        name = Displayname,
                    };

                    (string mess, User user) = await AuthService.ins.SignUp(u);
                    UserDialogs.Instance.HideLoading();
                    if (user == null)
                    {
                        UserDialogs.Instance.Toast(mess);
                        return;
                    }

                    //NAVIGATE TO HOME
                    await Application.Current.MainPage.DisplayAlert(AppResources.alert, AppResources.createaccountsuccessfully, "OK");
                    Application.Current.MainPage = new NavigationPage(new LoginScreen());
                    await Application.Current.MainPage.Navigation.PopToRootAsync();
                }
                else
                {
                    UserDialogs.Instance.Toast(AppResources.wrongverificationcode);
                }

            });
            PopToSignUpClickCM = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });

        }


        (String, bool) isValidateData()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(RePassword) || string.IsNullOrEmpty(Displayname))
            {
                return (AppResources.pleaseenteryourinformation, false);
            }
            if (Password != RePassword)
            {
                return (AppResources.incorrectpassword, false);
            }
            if (Password.Length < 6)
            {
                return (AppResources.atleast6char, false);
            }
            if (!Validation.IsValidEmail(Email))
            {
                return (AppResources.wrongemailorpassword, false);
            }
            return ("", true);
        }
    }
}
