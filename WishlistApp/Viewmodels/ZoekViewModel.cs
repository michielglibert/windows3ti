using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;
using WishlistApp.Annotations;
using WishlistApp.Models;
using WishlistApp.Utils;
using WishlistApp.Views;

namespace WishlistApp.Viewmodels
{
    public class ZoekViewModel : INotifyPropertyChanged
    {
        public RelayCommand GoToGebruiker => new RelayCommand((gebruiker) => MainPage.Frame.Navigate(typeof(Profiel), gebruiker));
        public RelayCommand GoToWishlist => new RelayCommand((wishlist) => MainPage.Frame.Navigate(typeof(WishlistDetail), wishlist));

        private ObservableCollection<Gebruiker> _gebruikersLijst;

        public ObservableCollection<Gebruiker> ResultGebruikersLijst
        {
            get { return _gebruikersLijst; }
            set
            {
                _gebruikersLijst = value;
                OnPropertyChanged(nameof(ResultGebruikersLijst));
            }
        }

        private ObservableCollection<Wishlist> _wishlistLijst;

        public ObservableCollection<Wishlist> ResultWishlistLijst
        {
            get { return _wishlistLijst; }
            set
            {
                _wishlistLijst = value;
                OnPropertyChanged(nameof(ResultWishlistLijst));
            }
        }

        private string _zoekError;
        public string ZoekError
        {
            get { return _zoekError; }
            set
            {
                _zoekError = value;
                OnPropertyChanged(nameof(ZoekError));
            }
        }

        public List<string> ZoekSoorten { get; set; }
        public string ZoekString { get; set; }
        public string GeselecteerdeSoort { get; set; }
        public RelayCommand ZoekCommand { get; set; }
        public RelayCommand GoToProfiel { get; set; }

        public ZoekViewModel()
        {
            ZoekSoorten = new List<string> { "Gebruiker", "Wishlist" };
            ZoekCommand = new RelayCommand((param) => StelLijstIn(param));
            //GoToProfiel = new RelayCommand((frame) => Zoeken.Frame.Navigate(typeof(Profiel)));
        }

        /*public void ListViewOnItemClick(ItemClickEventArgs args)
        {
            if (args.ClickedItem.GetType() == typeof(Gebruiker))
            {
                Gebruiker gebruiker = (Gebruiker) args.ClickedItem;
                Debug.WriteLine(gebruiker);
            }
            else
            {
                Wishlist wishlist = (Wishlist) args.ClickedItem;
            }
        }*/

        public async void GetGebruikersLijstOpUsername(string zoekString)
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", localSettings.Values["token"].ToString());
            var json = await client.GetStringAsync(new Uri("http://localhost:58253/api/Gebruikers/search?naam=" + zoekString));
            var result = JsonConvert.DeserializeObject<ObservableCollection<Gebruiker>>(json);
            if (result.Count == 0)
            {
                ZoekError = "Gebruiker met naam: \"" + zoekString + "\" bestaat niet.";
            }
            ResultGebruikersLijst = result;
        }

        public async void GetWishlistsLijstOpUsername(string zoekString)
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", localSettings.Values["token"].ToString());
            var json = await client.GetStringAsync(new Uri("http://localhost:58253/api/Wishlists/search?naam=" + zoekString));
            var result = JsonConvert.DeserializeObject<ObservableCollection<Wishlist>>(json);
            if (result == null)
            {
                ZoekError = "Wishist met naam: \"" + zoekString + "\" bestaat niet.";
            }
            ResultWishlistLijst = result;
            Debug.WriteLine(ZoekError);
        }

        private void StelLijstIn(object zoekString)
        {
            Debug.WriteLine(zoekString.ToString());
            Debug.WriteLine(GeselecteerdeSoort);
            Debug.WriteLine("ZoekButton geklikt");

            if (GeselecteerdeSoort.Equals("Gebruiker"))
            {
                if (!zoekString.ToString().Equals(""))
                {
                    GetGebruikersLijstOpUsername(zoekString.ToString());

                }
                else ZoekError = "Gelieve een Gebruikersnaam of Wishlistnaam in te geven.";
            }
            else
            {
                if (!zoekString.ToString().Equals(""))
                {
                    GetWishlistsLijstOpUsername(zoekString.ToString());
                }
                else ZoekError = "Gelieve een Gebruikersnaam of Wishlistnaam in te geven.";
            }
            Debug.WriteLine(ZoekError);
            Debug.WriteLine(_gebruikersLijst);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /*public List<Gebruiker> GenerateGebruikersList()
        {
            Gebruiker g1 = new Gebruiker { Username = "Frank" };
            Gebruiker g2 = new Gebruiker { Username = "Henk" };
            Gebruiker g3 = new Gebruiker { Username = "Mariette" };
            Gebruiker g4 = new Gebruiker { Username = "Jean-Claude" };
            Gebruiker g5 = new Gebruiker { Username = "Wilhelm" };

            List<Gebruiker> gebruikers = new List<Gebruiker>{g1, g2, g3, g4, g5};
            return gebruikers;
        }

        private List<Wishlist> GenerateWishlists()
        {
            Wishlist w1 = new Wishlist { Naam = "Fatima's Hanoeka Wishlist", Ontvanger = new Gebruiker { Username = "Fatima" } };
            Wishlist w2 = new Wishlist { Naam = "Bob's vrijgezellenfeest Wensenlijst", Ontvanger = new Gebruiker { Username = "Bob" } };
            Wishlist w3 = new Wishlist { Naam = "Thomas's uit de kast gekomen Wensenlijst", Ontvanger = new Gebruiker { Username = "Thomas" } };

            List<Wishlist> wishlists = new List<Wishlist> { w1, w2, w3 };
            return wishlists;
        }

        public IEnumerable<Gebruiker> ZoekGebruikersMetUsername(string zoekString)
        {
            return _gebruikersLijst.Where(g => g.Username == ZoekString);
        }
        */
    }
}
