using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WishlistApp.Annotations;
using WishlistApp.Models;
using WishlistApp.Utils;

namespace WishlistApp.Viewmodels
{
    class NewWensViewModel:INotifyPropertyChanged
    {
        public HttpClient Client { get; set; }
        public Wishlist Wishlist { get; set; }

        public RelayCommand NewWensCommand => new RelayCommand((wens) => { WensToevoegen((Wens) wens); });

        public NewWensViewModel()
        {
            Client = new HttpClient();
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", localSettings.Values["token"].ToString());
        }

        public async void WensToevoegen(Wens wens)
        {
            var json = JsonConvert.SerializeObject(wens);
            await Client.PostAsync(new Uri("http://localhost:58253/api/Wishlists/" + Wishlist.Id + "/Wensen"),
                new StringContent(json, Encoding.UTF8, "application/json"));

            MainPage.Frame.Navigate(typeof(WishlistDetailViewModel), Wishlist);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
