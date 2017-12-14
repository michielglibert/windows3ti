using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;
using WishlistApp.Annotations;
using WishlistApp.Models;
using WishlistApp.Utils;
using WishlistApp.Views;

namespace WishlistApp.Viewmodels
{
    class WishlistViewModel:INotifyPropertyChanged
    {
        public RelayCommand GoToWishlist => new RelayCommand((wishlist) => MainPage.Frame.Navigate(typeof(WishlistDetail), wishlist));
        public RelayCommand OpenContentDialog => new RelayCommand((dialog) => { ShowDialog(); });
        public RelayCommand WishlistMakenCommand => new RelayCommand((naam) => WishlistMaken((string) naam));

        public TextBox TextBox { get; set; } = new TextBox();

        private ObservableCollection<Wishlist> _eigenWishlists;

        public ObservableCollection<Wishlist> EigenWishlists
        {
            get => _eigenWishlists;
            set { _eigenWishlists = value; OnPropertyChanged(nameof(EigenWishlists)); }
        }

        private ObservableCollection<Wishlist> _wishlists;

        public ObservableCollection<Wishlist> Wishlists
        {
            get => _wishlists;
            set { _wishlists = value; OnPropertyChanged(nameof(Wishlists)); }
        }

        public HttpClient Client { get; set; }

        public WishlistViewModel()
        {
            //Client aanmaken
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", localSettings.Values["token"].ToString());

            //Data ophalen
            GetEigenWishlists();
            GetWishlists();
        }

        public async void GetEigenWishlists()
        {
            var json = await Client.GetStringAsync(new Uri("http://localhost:58253/api/EigenWishlists"));
            var lst = JsonConvert.DeserializeObject<ObservableCollection<Wishlist>>(json);
            EigenWishlists = lst;
        }

        public async void GetWishlists()
        {
            var json = await Client.GetStringAsync(new Uri("http://localhost:58253/api/Wishlists"));
            var lst = JsonConvert.DeserializeObject<ObservableCollection<Wishlist>>(json);
            Wishlists = lst;
        }

        public async void WishlistMaken(string naam)
        {
            Wishlist wishlist = new Wishlist(naam);

            var json = JsonConvert.SerializeObject(wishlist);
            var res = await Client.PostAsync(new Uri("http://localhost:58253/api/Wishlists"),
                new StringContent(json, Encoding.UTF8, "application/json"));

            var jsonResp = await res.Content.ReadAsStringAsync();
            var newWishlist = JsonConvert.DeserializeObject<Wishlist>(jsonResp);

            EigenWishlists.Add(newWishlist);
        }
        
        public async void ShowDialog()
        {
            ContentDialog uitnodigingDialog = new ContentDialog
            {
                Title = "Nieuwe wishlist naam",
                Content = TextBox,
                PrimaryButtonText = "Aanmaken",
                SecondaryButtonText = "Annuleren",
                PrimaryButtonCommand = WishlistMakenCommand,
                PrimaryButtonCommandParameter = TextBox.Text
            };

            await uitnodigingDialog.ShowAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
