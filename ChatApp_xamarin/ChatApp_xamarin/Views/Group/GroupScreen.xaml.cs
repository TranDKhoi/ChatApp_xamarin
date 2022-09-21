using ChatApp_xamarin.ViewModels.Chat;
using ChatApp_xamarin.ViewModels.Group;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChatApp_xamarin.Views.Group
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupScreen : ContentPage
    {
        public GroupScreen()
        {
            InitializeComponent();
        }

        private void listFriends_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            CollectionView cl = sender as CollectionView;
            var vm = (GroupViewModel)this.BindingContext;
            vm.AddToMemberQueueCM.Execute(cl.SelectedItem);
        }

        protected override void OnAppearing()
        {
            var vm = Application.Current.Resources["ChatVM"] as ChatViewModel;

            if (vm.CurrentRoom.memberId.Count != 2)
                groupNameEntry.IsVisible = false;
        }
    }
}