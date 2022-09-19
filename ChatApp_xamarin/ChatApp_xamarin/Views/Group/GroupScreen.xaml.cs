using ChatApp_xamarin.ViewModels.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private void listFriends_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void listFriends_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            CollectionView cl = sender as CollectionView;
            var vm = (GroupViewModel)this.BindingContext;
            vm.AddToMemberQueueCM.Execute(cl.SelectedItem);
        }
    }
}