using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;
using WishlistApp.Models;
using WishlistApp.Utils;

namespace WishlistApp.Viewmodels
{
    public class ProfielViewModel : ViewModelBase
    {
        public ObservableCollection<Wishlist> EigenWishlists { get; set; }
        public ObservableCollection<Wishlist> OntvangenWishlists { get; set; }
        private Gebruiker _gebruiker;
        public ObservableCollection<Wishlist> WishlistsIngelogdeGebruiker { get; set; }

        public Gebruiker Gebruiker
        {
            get { return _gebruiker; }
            set
            {
                _gebruiker = value;
                RaisePropertyChanged("Gebruiker");
            }
        }

        public string Naam
        {
            get { return Gebruiker.Username; }
        }

        private Wishlist _selectedWishlist;

        public Wishlist SelectedWishlist {
            get { return _selectedWishlist; }
            set
            {
                _selectedWishlist = value;
                RaisePropertyChanged("SelectedWishlist");
            } 
        }

        public RelayCommand JoinOrLeaveCommand { get; set; }
        public RelayCommand NodigUitCommand { get; set; }

        public RelayCommand OpenContentDialogCommand { get; set; }

        public ProfielViewModel()
        {
            //TODO: API CALLS
            //EigenWishlists = new ObservableCollection<Wishlist>(GenerateEigenWishlists());
            //OntvangenWishlists = new ObservableCollection<Wishlist>(GenerateAndereWishlists());
            //WishlistsIngelogdeGebruiker = new ObservableCollection<Wishlist>(GenerateWishlistsIngelogdeGebruiker());
            //Gebruiker = new Gebruiker { Naam = "Koen" };


            JoinOrLeaveCommand = new RelayCommand((param) => JoinOrLeaveWishlist(param as Wishlist));
            NodigUitCommand = new RelayCommand(o => NodigUit(SelectedWishlist));
            OpenContentDialogCommand = new RelayCommand((param) => OpenContentDialog(param as ContentDialog));
        }

        public void initData()
        {
            GetGebruiker();
            GetEigenWishlistsVanProfiel();
            GetAndereWishlistsVanProfiel();
            GetWishlistsIngelogdeGebruiker();
        }

        private void NodigUit(Wishlist wishlist)
        {
            //TODO: NodigUit() implementeren, check op nul (combobox = leeg)
            //wishlist.NodigUit();
            System.Diagnostics.Debug.WriteLine("NodigUit werkt");
            System.Diagnostics.Debug.WriteLine(SelectedWishlist.Naam);
        }

        private void OpenContentDialog(ContentDialog dialog)
        {
            //TODO Andere dialoog tonen indien nog geen wishlist (kan dus niet uitnodigen)
            System.Diagnostics.Debug.WriteLine("OpenContentDialogCommand werkt");
            dialog.ShowAsync();
        }

        private void JoinOrLeaveWishlist(Wishlist wishlist)
        {
            //TODO: JoinOrLeave() implementeren
            //wishlist.JoinOrLeave();
            System.Diagnostics.Debug.WriteLine("JoinOrLeave werkt");
            System.Diagnostics.Debug.WriteLine(wishlist.Naam);
        }

        public List<Wishlist> GenerateEigenWishlists()
        {
            Wishlist w1 = new Wishlist {Naam = "Koen's Kerst Wishlist"};
            Wishlist w2 = new Wishlist {Naam = "Koen's Birthday Wishlist"};

            List<Wishlist> wishlists = new List<Wishlist> {w1, w2};
            return wishlists;

        }
        public async void GetEigenWishlistsVanProfiel()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", localSettings.Values["token"].ToString());
            var json = await client.GetStringAsync(new Uri("http://localhost:58253/api/WishlistsFromGebruiker/" + Gebruiker.Id));
            var lst = JsonConvert.DeserializeObject<ObservableCollection<Wishlist>>(json);
            EigenWishlists = lst;
        }

        public List<Wishlist> GenerateAndereWishlists()
        {
            Wishlist w1 = new Wishlist { Naam = "Fatima's Hanoeka Wishlist", Ontvanger = new Gebruiker {Username = "Fatima"}};
            Wishlist w2 = new Wishlist { Naam = "Bob's vrijgezellenfeest Wensenlijst", Ontvanger = new Gebruiker { Username = "Bob" } };
            Wishlist w3 = new Wishlist { Naam = "Thomas's uit de kast gekomen Wensenlijst", Ontvanger = new Gebruiker { Username = "Thomas" } };

            List<Wishlist> wishlists = new List<Wishlist> { w1, w2, w3 };
            return wishlists;

        }

        public async void GetAndereWishlistsVanProfiel()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", localSettings.Values["token"].ToString());
            var json = await client.GetStringAsync(new Uri("http://localhost:58253/api/DeelnemendeWishlistsFromGebruiker/" + Gebruiker.Id));
            var lst = JsonConvert.DeserializeObject<ObservableCollection<Wishlist>>(json);
            OntvangenWishlists = lst;
        }

        public List<Wishlist> GenerateWishlistsIngelogdeGebruiker()
        {
            Wishlist w1 = new Wishlist { Naam = "Verjaardag Jef", Ontvanger = new Gebruiker{ Username = "Jef"} };
            Wishlist w2 = new Wishlist { Naam = "Barmitsha Jef", Ontvanger = new Gebruiker { Username = "Jef" } };

            List<Wishlist> wishlists = new List<Wishlist> { w1, w2 };
            return wishlists;

        }

        public async void GetWishlistsIngelogdeGebruiker()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", localSettings.Values["token"].ToString());
            var json = await client.GetStringAsync(new Uri("http://localhost:58253/api/EigenWishlists"));
            var lst = JsonConvert.DeserializeObject<ObservableCollection<Wishlist>>(json);
            WishlistsIngelogdeGebruiker = lst;
        }

        public async void GetGebruiker()
        {
            //TODO vaste id aanpassen
            int id = 1;

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", localSettings.Values["token"].ToString());
            var json = await client.GetStringAsync(new Uri("http://localhost:58253/api/Gebruikers/" + id ));
            var gebruiker = JsonConvert.DeserializeObject<Gebruiker>(json);
            Gebruiker = gebruiker;
        }
    }
}
