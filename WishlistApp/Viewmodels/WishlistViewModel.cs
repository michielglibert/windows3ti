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
using Newtonsoft.Json;
using WishlistApp.Annotations;
using WishlistApp.Models;

namespace WishlistApp.Viewmodels
{
    class WishlistViewModel:INotifyPropertyChanged
    {

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

        public WishlistViewModel()
        {
            GetEigenWishlists();
            GetWishlists();
        }

        public async void GetEigenWishlists()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", localSettings.Values["token"].ToString());
            var json = await client.GetStringAsync(new Uri("http://localhost:58253/api/EigenWishlists"));
            var lst = JsonConvert.DeserializeObject<ObservableCollection<Wishlist>>(json);
            EigenWishlists = lst;
        }

        public async void GetWishlists()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", localSettings.Values["token"].ToString());
            var json = await client.GetStringAsync(new Uri("http://localhost:58253/api/Wishlists"));
            var lst = JsonConvert.DeserializeObject<ObservableCollection<Wishlist>>(json);
            Wishlists = lst;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
