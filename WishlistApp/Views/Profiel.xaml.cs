using Windows.UI.Xaml.Controls;
using WishlistApp.Viewmodels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WishlistApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Profiel : Page
    {
        public ProfielViewModel ViewModel { get; set; }
        public Profiel()
        {
            this.InitializeComponent();
            this.ViewModel = new ProfielViewModel();
        }
    }
}
