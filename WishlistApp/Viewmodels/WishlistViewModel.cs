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
        public ObservableCollection<Wishlist> EigenWishlists { get; set; }

        public WishlistViewModel()
        {
            GetEigenWishlists();
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
