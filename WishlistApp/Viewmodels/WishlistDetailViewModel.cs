using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;
using WishlisApp.Models;
using WishlistApp.Annotations;
using WishlistApp.Models;
using WishlistApp.Utils;

namespace WishlistApp.Viewmodels
{
    class WishlistDetailViewModel:INotifyPropertyChanged
    {
        public HttpClient Client { get; set; }

        public RelayCommand OpenRequestDialogCommand => new RelayCommand((request) => { ShowRequestDialog((Request)request); });
        public RelayCommand RequestAanvaarden => new RelayCommand((request) => { RequestBeantwoorden((Request)request, true); });
        public RelayCommand RequestAfwijzen => new RelayCommand((request) => { RequestBeantwoorden((Request)request, false); });

        private Wishlist _wishlist;
        public Wishlist Wishlist
        {
            get { return _wishlist; }
            set { _wishlist = value; OnPropertyChanged(nameof(Wishlist)); }
        }

        public WishlistDetailViewModel()
        {
            Client = new HttpClient();
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", localSettings.Values["token"].ToString());
        }
        
        public async void RequestBeantwoorden(Request r, bool antwoord)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", localSettings.Values["token"].ToString());
            var json = JsonConvert.SerializeObject(new { Antwoord = antwoord });

            HttpRequestMessage request = new HttpRequestMessage
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
                Method = HttpMethod.Delete,
                RequestUri = new Uri("http://localhost:58253/api/Requests/" + r.Id)
            };

            var resp = await client.SendAsync(request);

            if (resp.StatusCode == HttpStatusCode.NoContent)
            {
                Wishlist.Requests.Remove(r);
            }
        }

        public async void ShowRequestDialog(Request request)
        {
            ContentDialog requestDialog = new ContentDialog
            {
                Title = "Request",
                Content = "Wat wil je met deze Request doen?",
                PrimaryButtonText = "Aanvaarden",
                SecondaryButtonText = "Afwijzen",
                PrimaryButtonCommand = RequestAanvaarden,
                PrimaryButtonCommandParameter = request,
                SecondaryButtonCommand = RequestAfwijzen,
                SecondaryButtonCommandParameter = request
            };

            await requestDialog.ShowAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
