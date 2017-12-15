using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WishlistApp.Models;
using WishlistApp.Viewmodels;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WishlistApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Profiel : Page
    {
        public Profiel()
        {
            
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ProfielViewModel.Gebruiker = (Gebruiker) e.Parameter;
            ProfielViewModel.InitData();
            base.OnNavigatedTo(e);
        }
    }
}
