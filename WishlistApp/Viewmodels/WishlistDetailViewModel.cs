using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
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
using Wens = WishlistApp.Models.Wens;


namespace WishlistApp.Viewmodels
{
    class WishlistDetailViewModel : INotifyPropertyChanged
    {
        public HttpClient Client { get; set; }
        
        public RelayCommand GoToWens => new RelayCommand((wens) => MainPage.Frame.Navigate(typeof(WensView), wens));
        public RelayCommand OpenRequestDialogCommand => new RelayCommand((request) => { ShowRequestDialog((Request)request); });
        public RelayCommand RequestAanvaarden => new RelayCommand((request) => { RequestBeantwoorden((Request)request, true); });
        public RelayCommand RequestAfwijzen => new RelayCommand((request) => { RequestBeantwoorden((Request)request, false); });
        public RelayCommand AddNewWens => new RelayCommand(WensToevoegen);
        public RelayCommand EditCommand => new RelayCommand((wens) => MainPage.Frame.Navigate(typeof(WensWijzigen), wens));
        public RelayCommand RemoveCommand => new RelayCommand((wens) => { WensVerwijderen((Wens)wens); });


        private string _nieuweWensTitel;
        public string NieuweWensTitel
        {
            get { return _nieuweWensTitel;}
            set { _nieuweWensTitel = value; OnPropertyChanged(nameof(NieuweWensTitel)); }
        }

        private string _nieuweWensOmschrijving;
        public string NieuweWensOmschrijving
        {
            get { return _nieuweWensOmschrijving; }
            set { _nieuweWensOmschrijving = value; OnPropertyChanged(nameof(NieuweWensOmschrijving)); }
        }

        private Categorie _geselecteerdeCategorie;
        public Categorie GeselecteerdeCategorie
        {
            get { return _geselecteerdeCategorie; }
            set { _geselecteerdeCategorie = value; OnPropertyChanged(nameof(GeselecteerdeCategorie)); }
        }

        public List<Categorie> CategorieLijst { get; set; }

        private Wishlist _wishlist;
        public Wishlist Wishlist
        {
            get { return _wishlist; }
            set { _wishlist = value; OnPropertyChanged(nameof(Wishlist)); }
        }

        private ObservableCollection<Wens> _wensen;
        public ObservableCollection<Wens> Wensen
        {
            get { return _wensen; }
            set { _wensen = value; OnPropertyChanged(nameof(Wensen)); }
        }


        private ObservableCollection<Gebruiker> _kopers;
        public ObservableCollection<Gebruiker> Kopers
        {
            get { return _kopers; }
            set { _kopers = value; OnPropertyChanged(nameof(Kopers)); }
        }
        private ObservableCollection<Uitnodiging> _uitnodigingen;

        public ObservableCollection<Uitnodiging> Uitnodigingen
        {
            get { return _uitnodigingen; }
            set { _uitnodigingen = value; OnPropertyChanged(nameof(Uitnodigingen)); }
        }

        private ObservableCollection<Request> _requests;

        public ObservableCollection<Request> Requests
        {
            get { return _requests; }
            set { _requests = value; OnPropertyChanged(nameof(Requests)); }
        }

        public WishlistDetailViewModel()
        {
            CategorieLijst = Enum.GetValues(typeof(Categorie)).Cast<Categorie>().ToList();
            Client = new HttpClient();
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", localSettings.Values["token"].ToString());
        }

        private async void WensToevoegen(object o)
        {
            Debug.WriteLine("Wens toevoegen gedrukt");
            Debug.WriteLine(o);
            Wens wens = new Wens(NieuweWensTitel, NieuweWensOmschrijving, GeselecteerdeCategorie);
            Debug.WriteLine(wens);
            var json = JsonConvert.SerializeObject(wens);
            var res = await Client.PostAsync(new Uri("http://localhost:58253/api/Wishlists/" + Wishlist.Id + "/Wensen"),
                new StringContent(json, Encoding.UTF8, "application/json"));

            var jsonResp = await res.Content.ReadAsStringAsync();
            var newWens = JsonConvert.DeserializeObject<Wens>(jsonResp);
            Wensen.Add(newWens);
        }

        private async void WensVerwijderen(Wens wens)
        {
            var res = await Client.DeleteAsync(new Uri("http://localhost:58253/api/Wensen/" + wens.Id));
            Wensen.Remove(wens);
        }
        public void InitData()
        {
            GetWishlist();
            GetWensen();
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

        public void GetWensen()
        {
            Wensen = new ObservableCollection<Wens>(Wishlist.Wensen);
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
