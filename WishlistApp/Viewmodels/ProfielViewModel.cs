using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;
using WishlisApp.Models;
using WishlistApp.Models;
using WishlistApp.Utils;

namespace WishlistApp.Viewmodels
{
    public class ProfielViewModel : ViewModelBase
    {
        #region Properties

        public ObservableCollection<Wishlist> EigenWishlists { get; set; }
        public ObservableCollection<Wishlist> OntvangenWishlists { get; set; }
        public ObservableCollection<Wishlist> WishlistsIngelogdeGebruiker { get; set; }

        private Gebruiker _gebruiker;
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
        public Wishlist SelectedWishlist
        {
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
        public RelayCommand CloseContentDialogCommand { get; set; }

        #endregion

        public ProfielViewModel()
        {
            //EigenWishlists = new ObservableCollection<Wishlist>(GenerateEigenWishlists());
            //OntvangenWishlists = new ObservableCollection<Wishlist>(GenerateAndereWishlists());
            //WishlistsIngelogdeGebruiker = new ObservableCollection<Wishlist>(GenerateWishlistsIngelogdeGebruiker());
            //Gebruiker = new Gebruiker { Naam = "Koen" };

            JoinOrLeaveCommand = new RelayCommand((param) => JoinOrLeaveWishlist(param as Wishlist));
            NodigUitCommand = new RelayCommand(o => NodigUit(SelectedWishlist));
            OpenContentDialogCommand = new RelayCommand((param) => OpenContentDialog(param as ContentDialog));
            CloseContentDialogCommand = new RelayCommand(param => CloseContentDialog(param as ContentDialog));
        }

        public void InitData()
        {
            GetEigenWishlistsVanProfiel();
            GetAndereWishlistsVanProfiel();
            GetWishlistsIngelogdeGebruiker();
        }

        private void NodigUit(Wishlist wishlist)
        {
            //TODO: NodigUit() implementeren, check op nul (combobox = leeg) en controleren
            Debug.WriteLine("NodigUit werkt");
            if (wishlist != null)
            {
                Debug.WriteLine(SelectedWishlist.Naam);
                Uitnodiging uitnodiging = new Uitnodiging(wishlist, Gebruiker);
                PostUitnodiging(uitnodiging);
            } 
        }

        private void OpenContentDialog(ContentDialog dialog)
        {
            //TODO Andere dialoog tonen indien nog geen wishlist (kan dus niet uitnodigen)
            Debug.WriteLine("OpenContentDialogCommand werkt");
            dialog.ShowAsync();
        }

        private void CloseContentDialog(ContentDialog dialog)
        {
            dialog.Hide();
        }

        private void JoinOrLeaveWishlist(Wishlist wishlist)
        {
            //TODO: JoinOrLeave() implementeren
            Debug.WriteLine("JoinOrLeave werkt");
            Debug.WriteLine(wishlist.Naam);
            switch (wishlist.JoinOrLeaveText)
            {
                case "Verlaat wishlist":
                    // Show popup
                    // Remove from wishlist
                    break;
                case "Annuleer aanvraag":
                    // Remove request
                    break;
                case "Accepteer uitnodiging":
                    // Accepteer uitnodiging
                    break;
                case "Vraag aan":
                    // Add request
                    break;
            }
        }

        private bool IsHuidigeGebruikerDeelVanWishlist(Wishlist wishlist)
        {
            //TODO: is huidige gebruiker deel van wishlist? (controleren)
            List<string> usernames = new List<string>();
            wishlist.Kopers.ForEach(koper => usernames.Add(koper.Gebruiker.Username));

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string ingelogdeGebruikerUsername = localSettings.Values["username"].ToString();

            return usernames.Contains(ingelogdeGebruikerUsername);
        }

        private bool IsHuigeGebruikerReedsUitgenodigd(Wishlist wishlist)
        {
            //TODO: is huidige gebruiker reeds uitgenodigd? (controleren)
            List<string> usernames = new List<string>();
            wishlist.VerzondenUitnodigingen.ForEach(uitnodiging => usernames.Add(uitnodiging.Gebruiker.Username));

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string ingelogdeGebruikerUsername = localSettings.Values["username"].ToString();

            return usernames.Contains(ingelogdeGebruikerUsername);
        }

        private bool HeeftHuidigeGebruikerReedsAanvraagIngediend(Wishlist wishlist)
        {
            //TODO: heeft huidige gebruiker reeds een aanvraag ingediend?
            List<string> usernames = new List<string>();
            wishlist.Requests.ForEach(request => usernames.Add(request.Gebruiker.Username));

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string ingelogdeGebruikerUsername = localSettings.Values["username"].ToString();

            return usernames.Contains(ingelogdeGebruikerUsername);
        }

        private string BepaalJoinOrLeaveButtonString(Wishlist wishlist)
        {
            if (IsHuidigeGebruikerDeelVanWishlist(wishlist))
            {
                return "Verlaat wishlist";
            }
            else if (HeeftHuidigeGebruikerReedsAanvraagIngediend(wishlist))
            {
                return "Annuleer aanvraag";
            }
            else if (IsHuigeGebruikerReedsUitgenodigd(wishlist))
            {
                return "Accepteer uitnodiging";
            }
            else
            {
                return "Vraag aan";
            }
        }

        #region API Calls

        public async void GetEigenWishlistsVanProfiel()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", localSettings.Values["token"].ToString());
            var json = await client.GetStringAsync(new Uri("http://localhost:58253/api/WishlistsFromGebruiker/" + Gebruiker.Id));
            var lst = JsonConvert.DeserializeObject<ObservableCollection<Wishlist>>(json);

            EigenWishlists = lst;

            //TODO: controleren of dit werkt
            foreach (var eigenWishlist in EigenWishlists)
            {
                eigenWishlist.JoinOrLeaveText = BepaalJoinOrLeaveButtonString(eigenWishlist);
            }
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

        public async void GetWishlistsIngelogdeGebruiker()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", localSettings.Values["token"].ToString());
            var json = await client.GetStringAsync(new Uri("http://localhost:58253/api/EigenWishlists"));
            var lst = JsonConvert.DeserializeObject<ObservableCollection<Wishlist>>(json);
            WishlistsIngelogdeGebruiker = lst;
        }

        private async void PostUitnodiging(Uitnodiging uitnodiging)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", localSettings.Values["token"].ToString());
            var jsonUitnodiging = JsonConvert.SerializeObject(uitnodiging);
            var content = new StringContent(jsonUitnodiging, Encoding.UTF8, "application/json");

            await client.PostAsync(new Uri("http://localhost:58253/api/Uitnodigingen"), content);

            //TODO melding geven indien succesvol
        }

        //public async Task GetGebruiker()
        //{
        //    //TODO vaste id aanpassen
        //    int id = 1;

        //    var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        //    HttpClient client = new HttpClient();
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", localSettings.Values["token"].ToString());
        //    var json = await client.GetStringAsync(new Uri("http://localhost:58253/api/Gebruikers/" + id ));
        //    var gebruiker = JsonConvert.DeserializeObject<Gebruiker>(json);
        //    Gebruiker = gebruiker;
        //}

        #endregion

        #region DummydataGeneration

        //public List<Wishlist> GenerateWishlistsIngelogdeGebruiker()
        //{
        //    Wishlist w1 = new Wishlist { Naam = "Verjaardag Jef", Ontvanger = new Gebruiker { Username = "Jef" } };
        //    Wishlist w2 = new Wishlist { Naam = "Barmitsha Jef", Ontvanger = new Gebruiker { Username = "Jef" } };

        //    List<Wishlist> wishlists = new List<Wishlist> { w1, w2 };
        //    return wishlists;

        //}

        //public List<Wishlist> GenerateAndereWishlists()
        //{
        //    Wishlist w1 = new Wishlist { Naam = "Fatima's Hanoeka Wishlist", Ontvanger = new Gebruiker { Username = "Fatima" } };
        //    Wishlist w2 = new Wishlist { Naam = "Bob's vrijgezellenfeest Wensenlijst", Ontvanger = new Gebruiker { Username = "Bob" } };
        //    Wishlist w3 = new Wishlist { Naam = "Thomas's uit de kast gekomen Wensenlijst", Ontvanger = new Gebruiker { Username = "Thomas" } };

        //    List<Wishlist> wishlists = new List<Wishlist> { w1, w2, w3 };
        //    return wishlists;

        //}

        //public List<Wishlist> GenerateEigenWishlists()
        //{
        //    Wishlist w1 = new Wishlist { Naam = "Koen's Kerst Wishlist" };
        //    Wishlist w2 = new Wishlist { Naam = "Koen's Birthday Wishlist" };

        //    List<Wishlist> wishlists = new List<Wishlist> { w1, w2 };
        //    return wishlists;

        //}

        #endregion

    }
}
