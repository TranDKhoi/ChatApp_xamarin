using Acr.UserDialogs;
using ChatApp_xamarin.Models;
using ChatApp_xamarin.Resources;
using ChatApp_xamarin.Services;
using ChatApp_xamarin.Utils;
using ChatApp_xamarin.Views.Authentication.ForgotPass;
using ChatApp_xamarin.Views.Authentication.LoginScreen;
using System.Windows.Input;
using Xamarin.Forms;

namespace ChatApp_xamarin.ViewModels.Authentication
{
    public class ForgotPassViewModel : BaseViewModel
    {

        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }

        private string newPass;
        public string NewPass
        {
            get { return newPass; }
            set { newPass = value; OnPropertyChanged(); }
        }



        private int verifyCode;
        private User userNeedToReset;


        public ICommand PopToLoginScreenCM { get; set; }
        public ICommand SendcodeClickCM { get; set; }
        public ICommand PopToForgotPassScreenClickCM { get; set; }
        public ICommand PopUntilLoginScreenClickCM { get; set; }
        public ICommand ToResetPassScreenCM { get; set; }
        public ICommand ResetPassClickCM { get; set; }




        public ForgotPassViewModel()
        {
            PopToLoginScreenCM = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });

            PopToForgotPassScreenClickCM = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });

            PopUntilLoginScreenClickCM = new Command(async () =>
            {
                Application.Current.MainPage = new NavigationPage(new LoginScreen());
                await Application.Current.MainPage.Navigation.PopAsync();
            });

            SendcodeClickCM = new Command(async () =>
            {
                (string res, bool isS) = isValidEmail();

                if (isS)
                {
                    UserDialogs.Instance.ShowLoading();
                    (string resp, User u) = await UserService.ins.GetUserByEmail(email);
                    UserDialogs.Instance.HideLoading();


                    userNeedToReset = u;
                    if (u == null)
                    {
                        UserDialogs.Instance.Toast(AppResources.emaildoesnotexist);
                        return;
                    }

                    UserDialogs.Instance.ShowLoading();
                    verifyCode = await AuthService.ins.SendVerifyCode(Email.Trim());
                    UserDialogs.Instance.HideLoading();

                    await Application.Current.MainPage.DisplayAlert(AppResources.alert, AppResources.checkyouremail, "OK");
                    await Application.Current.MainPage.Navigation.PushAsync(new VerifyEmailForgotScreen());
                }
                else
                {
                    UserDialogs.Instance.Toast(res);
                    return;
                }
            });

            ToResetPassScreenCM = new Command(async (p) =>
            {
                Entry en = p as Entry;
                if (string.IsNullOrEmpty(en.Text)) return;
                if (int.Parse(en.Text.Trim()) == verifyCode)
                {
                    Application.Current.MainPage = new NavigationPage(new ResetPassScreen());
                    await Application.Current.MainPage.Navigation.PopToRootAsync();
                }
                else
                {
                    UserDialogs.Instance.Toast(AppResources.wrongverificationcode);
                }
            });

            ResetPassClickCM = new Command(async (p) =>
            {
                Entry en = p as Entry;

                if (string.IsNullOrEmpty(NewPass) || string.IsNullOrEmpty(en.Text))
                {
                    UserDialogs.Instance.Toast(AppResources.pleaseenteryourpassword);
                    return;
                }
                if (en.Text.Length < 6 || NewPass.Length < 6)
                {
                    UserDialogs.Instance.Toast(AppResources.atleast6char);
                    return;
                }
                if (en.Text != NewPass)
                {
                    UserDialogs.Instance.Toast(AppResources.incorrectpassword);
                    return;
                }

                (string res, bool isS) = await AuthService.ins.ResetPassword(userNeedToReset.id, NewPass);
                if (isS)
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.alert, res, "OK");
                    Application.Current.MainPage = new NavigationPage(new LoginScreen());
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    UserDialogs.Instance.Toast(res);
                }
            });
        }

        private (string, bool) isValidEmail()
        {
            if (string.IsNullOrEmpty(Email))
                return (AppResources.pleaseenteryouremail, false);
            if (!Validation.IsValidEmail(Email))
                return (AppResources.invalidemailformat, false);
            return ("", true);
        }
    }
}
