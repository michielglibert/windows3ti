using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WishlistApp.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WishlistApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WishlistDetail : Page
    {
        public WishlistDetail()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Vm.Wishlist = (Wishlist) e.Parameter;
            Vm.InitData();
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            Vm.GoToWens.Execute(e.ClickedItem);
        }

        private void EditTapped(object sender, RoutedEventArgs e)
        {
            //Wens wens = new Wens();
            Vm.EditCommand.Execute(ListViewWensen.SelectedItem);
        }

        private void RemoveTapped(object sender, TappedRoutedEventArgs e)
        {
            Vm.RemoveCommand.Execute(ListViewWensen.SelectedItem);
        }

        private void ViewTapped(object sender, TappedRoutedEventArgs e)
        {
            Vm.GoToWens.Execute(ListViewWensen.SelectedItem);
        }
    }
}
