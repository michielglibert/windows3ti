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
using WishlistApp.Views;
using Wens = WishlistApp.Views.Wens;

namespace WishlistApp.Viewmodels
{
    class WishlistDetailViewModel:INotifyPropertyChanged
    {
        public HttpClient Client { get; set; }

        public RelayCommand GoToWishlist => new RelayCommand((wishlist) => MainPage.Frame.Navigate(typeof(NewWens), this));
        public RelayCommand GoToWens => new RelayCommand((wens) => MainPage.Frame.Navigate(typeof(Wens), wens));
        public RelayCommand OpenRequestDialogCommand => new RelayCommand((request) => { ShowRequestDialog((Request)request); });
        public RelayCommand RequestAanvaarden => new RelayCommand((request) => { RequestBeantwoorden((Request)request, true); });
        public RelayCommand RequestAfwijzen => new RelayCommand((request) => { RequestBeantwoorden((Request)request, false); });

        private Wishlist _wishlist;
        public Wishlist Wishlist
        {
            get { return _wishlist; }
            set { _wishlist = value; OnPropertyChanged(nameof(Wishlist)); }
        }

        private ObservableCollection<Gebruiker> _kopers;
        public ObservableCollection<Gebruiker> Kopers
        {
            get => _kopers;
            set { _kopers = value; OnPropertyChanged(nameof(Kopers)); }
        }
        private ObservableCollection<Uitnodiging> _uitnodigingen;

        public ObservableCollection<Uitnodiging> Uitnodigingen
        {
            get => _uitnodigingen;
            set { _uitnodigingen = value; OnPropertyChanged(nameof(Uitnodigingen)); }
        }

        private ObservableCollection<Request> _requests;

        public ObservableCollection<Request> Requests
        {
            get => _requests;
            set { _requests = value; OnPropertyChanged(nameof(Requests)); }
        }

        public WishlistDetailViewModel()
        {
            Client = new HttpClient();
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", localSettings.Values["token"].ToString());
        }

        public void InitData()
        {
            GetWishlist();
            GetKopers();
            GetRequests();
            GetUitnodigingen();
        }

        public async void GetWishlist()
        {
            var json = await Client.GetStringAsync(new Uri("http://localhost:58253/api/Wishlists/" + Wishlist.Id));
            var wishlist = JsonConvert.DeserializeObject<Wishlist>(json);
            Wishlist = wishlist;
        }

        public async void GetKopers()
        {
            var json = await Client.GetStringAsync(new Uri("http://localhost:58253/api/Wishlists/" + Wishlist.Id + "/Kopers"));
            var lst = JsonConvert.DeserializeObject<ObservableCollection<Gebruiker>>(json);
            Kopers = lst;
        }

        public async void GetUitnodigingen()
        {
            var json = await Client.GetStringAsync(new Uri("http://localhost:58253/api/Wishlists/" + Wishlist.Id + "/Uitnodigingen"));
            var lst = JsonConvert.DeserializeObject<ObservableCollection<Uitnodiging>>(json);
            Uitnodigingen = lst;
        }

        public async void GetRequests()
        {
            var json = await Client.GetStringAsync(new Uri("http://localhost:58253/api/Wishlists/" + Wishlist.Id + "/Requests"));
            var lst = JsonConvert.DeserializeObject<ObservableCollection<Request>>(json);
            Requests = lst;
        }
        
        public async void RequestBeantwoorden(Request r, bool antwoord)
        {
            var json = JsonConvert.SerializeObject(new { Antwoord = antwoord });

            HttpRequestMessage request = new HttpRequestMessage
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
                Method = HttpMethod.Delete,
                RequestUri = new Uri("http://localhost:58253/api/Requests/" + r.Id)
            };

            var resp = await Client.SendAsync(request);

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
