using Acr.UserDialogs;
using Azure.Storage.Blobs;
using ChatApp_xamarin.Models;
using ChatApp_xamarin.Resources;
using ChatApp_xamarin.Services;
using ChatApp_xamarin.Utils;
using ChatApp_xamarin.Views.Authentication.LoginScreen;
using ChatApp_xamarin.Views.BottomBarCustom;
using Microsoft.WindowsAzure.Storage.Blob;
using Plugin.Media;
using Plugin.Multilingual;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using static System.Net.WebRequestMethods;

namespace ChatApp_xamarin.ViewModels.Setting
{
    public class SettingViewModel : BaseViewModel
    {
        private User currentUser;
        public User CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; OnPropertyChanged(); }
        }


        private bool isDark;
        public bool IsDark
        {
            get { return isDark; }
            set { isDark = value; OnPropertyChanged(); }
        }

        private bool isVN;
        public bool ISVN
        {
            get { return isVN; }
            set { isVN = value; OnPropertyChanged(); }
        }

        private bool isSilent;
        public bool IsSilent
        {
            get { return isSilent; }
            set { isSilent = value; OnPropertyChanged(); }
        }

        private string avatarPath;
        public string AvatarPath
        {
            get { return avatarPath; }
            set { avatarPath = value; OnPropertyChanged(); }
        }




        public ICommand GetCurrentUserCM { get; set; }
        public ICommand ChangeThemeCM { get; set; }
        public ICommand ChangeLanguageCM { get; set; }
        public ICommand SignOutCM { get; set; }
        public ICommand ChangeSilentModeCM { get; set; }
        public ICommand PickAvatarCM { get; set; }

        public SettingViewModel()
        {

            GetCurrentUserCM = new Command(() =>
            {
                CurrentUser = GlobalData.ins.currentUser;
            });

            ChangeThemeCM = new Command(() =>
             {
                 if (Application.Current.UserAppTheme == OSAppTheme.Light)
                 {
                     Application.Current.UserAppTheme = OSAppTheme.Dark;
                     Preferences.Set("isDark", true);
                     IsDark = true;
                 }
                 else
                 {
                     Application.Current.UserAppTheme = OSAppTheme.Light;
                     Preferences.Set("isDark", false);
                     IsDark = false;
                 }
             });

            ChangeLanguageCM = new Command(() =>
            {
                var currentLang = CrossMultilingual.Current.CurrentCultureInfo.Name;
                if (currentLang == "en")
                {
                    CrossMultilingual.Current.CurrentCultureInfo = new System.Globalization.CultureInfo("vi");
                    Preferences.Set("isVN", true);
                    ISVN = true;
                }
                else
                {
                    CrossMultilingual.Current.CurrentCultureInfo = new System.Globalization.CultureInfo("en");
                    Preferences.Set("isVN", false);
                    ISVN = false;
                }
                Application.Current.MainPage = new NavigationPage(new BottomBarCustom());
            });

            ChangeSilentModeCM = new Command(() =>
            {
                GlobalData.ins.isSilentMode = !GlobalData.ins.isSilentMode;
                Preferences.Set("isSilent", GlobalData.ins.isSilentMode);
                IsSilent = GlobalData.ins.isSilentMode;
            });

            PickAvatarCM = new Command(async () =>
            {
                //pick image
                await CrossMedia.Current.Initialize();
                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });

                if (file == null)
                    return;

                //upload to azure
                UserDialogs.Instance.ShowLoading();
                AvatarPath = await StorageService.ins.UploadUserAvatar(file);
                UserDialogs.Instance.HideLoading();

                //update user in firebas
                UserService.ins.UpdateUserAvatar(AvatarPath);
            });

            SignOutCM = new Command(async () =>
            {
                if (await Application.Current.MainPage.DisplayAlert(AppResources.alert, AppResources.areyousurewanttosignout, "OK", AppResources.no))
                {
                    AuthService.ins.Logout(GlobalData.ins.currentUser.id);
                    Preferences.Remove("currentUser");
                    GlobalData.ins.currentUser = null;
                    Application.Current.MainPage = new NavigationPage(new LoginScreen());
                    await Application.Current.MainPage.Navigation.PopToRootAsync();
                }
            });
        }
    }
}
