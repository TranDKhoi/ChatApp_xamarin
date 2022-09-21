using Acr.UserDialogs;
using ChatApp_xamarin.Models;
using ChatApp_xamarin.Resources;
using ChatApp_xamarin.Services;
using ChatApp_xamarin.Utils;
using ChatApp_xamarin.Views.Authentication.ForgotPass;
using ChatApp_xamarin.Views.Authentication.SignUpScreen;
using ChatApp_xamarin.Views.BottomBarCustom;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

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

        private List<User> listData;

        public List<User> ListData
        {
            get { return listData; }
            set { listData = value; OnPropertyChanged(); }
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

                    GlobalData.ins.currentUser = user;
                    Preferences.Set("currentUser", user.id);
                    Application.Current.MainPage = new NavigationPage(new BottomBarCustom());
                    await Application.Current.MainPage.Navigation.PopToRootAsync();
                }
                else
                {
                    UserDialogs.Instance.Toast(AppResources.pleaseenteryourinformation);
                }
            });

            ForgotPassClickCM = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ForgotPassScreen());
            });

            SignUpClickCM = new Command(async (p) =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new SignUpScreen());
            });
        }


        bool isValidateData()
        {
            return !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
        }
    }
}






//NOTIFICATION
//var notification = new NotificationRequest
//{
//    BadgeNumber = 1,
//    Description = item.Object.email,
//    Title = item.Object.name,
//    ReturningData = "Test Data",
//    NotificationId = 253,
//    Android = new AndroidOptions
//    {
//        ChannelId = "AndroidMessChannel",
//        LaunchAppWhenTapped = true,
//        Priority = AndroidPriority.Max,
//    }
//};

//LocalNotificationCenter.Current.Show(notification);