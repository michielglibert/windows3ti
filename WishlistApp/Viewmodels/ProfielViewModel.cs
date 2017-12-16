using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
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

        private ObservableCollection<Wishlist> _eigenWishlists;
        public ObservableCollection<Wishlist> EigenWishlists
        {
            get { return _eigenWishlists; }
            set
            {
                _eigenWishlists = value;
                RaisePropertyChanged("EigenWishlists");
            }
        }

        private ObservableCollection<Wishlist> _ontvangenWishlists;
        public ObservableCollection<Wishlist> OntvangenWishlists
        {
            get { return _ontvangenWishlists; }
            set
            {
                _ontvangenWishlists = value;
                RaisePropertyChanged("OntvangenWishlists");
            }
        }

        private ObservableCollection<Wishlist> _wishlistsIngelogdeGebruiker;
        public ObservableCollection<Wishlist> WishlistsIngelogdeGebruiker
        {
            get { return _wishlistsIngelogdeGebruiker; }
            set
            {
                _wishlistsIngelogdeGebruiker = value;
                RaisePropertyChanged("WishlistsIngelogdeGebruiker");
            }
        }

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
        }

        public void InitData()
        {
            GetEigenWishlistsVanProfiel();
            GetAndereWishlistsVanProfiel();
            GetWishlistsIngelogdeGebruiker();
        }

        private void NodigUit(Wishlist wishlist)
        {
            Debug.WriteLine("NodigUit werkt");
            if (wishlist != null)
            {
                Debug.WriteLine(SelectedWishlist.Naam);
                Uitnodiging uitnodiging = new Uitnodiging(wishlist, Gebruiker);
                PostUitnodiging(uitnodiging);
                BepaalJoinOrLeaveButtonString(wishlist);
            } 
        }

        private void OpenContentDialog(ContentDialog dialog)
        {
            Debug.WriteLine("OpenContentDialogCommand werkt");
            dialog.ShowAsync();
        }

        private void JoinOrLeaveWishlist(Wishlist wishlist)
        {
            Debug.WriteLine("JoinOrLeave werkt");
            Debug.WriteLine(wishlist.Naam);

            if (IsHuidigeGebruikerDeelVanWishlist(wishlist))
            {
                // Show popup
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "Verlaat wishlist",
                    Content = "Bent u zeker dat u de wishlist " + wishlist.Naam + " wilt verlaten?",
                    PrimaryButtonText = "Ja",
                    PrimaryButtonCommand = new RelayCommand(param => VerlaatWishlist(wishlist)),
                    SecondaryButtonText = "Annuleer"
                };

                dialog.ShowAsync();
            }
            else if (HeeftHuidigeGebruikerReedsAanvraagIngediend(wishlist))
            {
                VerwijderAanvraag(wishlist);
            }
            else if (IsHuidigeGebruikerReedsUitgenodigd(wishlist))
            {
                AccepteerUitnodiging(wishlist);
            }
            else
            {
                VoegAanvraagToe(wishlist);
            }
        }

        private bool IsHuidigeGebruikerDeelVanWishlist(Wishlist wishlist)
        {
            List<string> usernames = new List<string>();
            wishlist.Kopers.ForEach(koper => usernames.Add(koper.Gebruiker.Username));

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string ingelogdeGebruikerUsername = localSettings.Values["username"].ToString();

            return usernames.Contains(ingelogdeGebruikerUsername);
        }

        private bool IsHuidigeGebruikerReedsUitgenodigd(Wishlist wishlist)
        {
            List<string> usernames = new List<string>();
            wishlist.VerzondenUitnodigingen.ForEach(uitnodiging => usernames.Add(uitnodiging.Gebruiker.Username));

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string ingelogdeGebruikerUsername = localSettings.Values["username"].ToString();

            return usernames.Contains(ingelogdeGebruikerUsername);
        }

        private bool HeeftHuidigeGebruikerReedsAanvraagIngediend(Wishlist wishlist)
        {
            List<string> usernames = new List<string>();
            wishlist.Requests.ForEach(request => usernames.Add(request.Gebruiker.Username));

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string ingelogdeGebruikerUsername = localSettings.Values["username"].ToString();

            return usernames.Contains(ingelogdeGebruikerUsername);
        }

        private void BepaalJoinOrLeaveButtonString(Wishlist wishlist)
        {
            if (IsHuidigeGebruikerDeelVanWishlist(wishlist))
            {
                wishlist.JoinOrLeaveText = "Verlaat wishlist";
            }
            else if (HeeftHuidigeGebruikerReedsAanvraagIngediend(wishlist))
            {
                wishlist.JoinOrLeaveText = "Annuleer aanvraag";
            }
            else if (IsHuidigeGebruikerReedsUitgenodigd(wishlist))
            {
                wishlist.JoinOrLeaveText = "Accepteer uitnodiging";
            }
            else
            {
                wishlist.JoinOrLeaveText = "Vraag aan";           
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

            foreach (var eigenWishlist in EigenWishlists)
            {
                BepaalJoinOrLeaveButtonString(eigenWishlist);
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

        private async void VerlaatWishlist(Wishlist wishlist)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", 
                localSettings.Values["token"].ToString());

            var resp = await client.DeleteAsync(new Uri("http://localhost:58253/api/Wishlists/" + wishlist.Id + "/Verlaten"));

            if (resp.IsSuccessStatusCode)
            {
                GetEigenWishlistsVanProfiel();
                Debug.WriteLine("Wishlist verlaten");
            }

        }

        private async void VerwijderAanvraag(Wishlist wishlist)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string username = localSettings.Values["username"].ToString();
            Request teVerwijderenRequest = wishlist.Requests.Find(request => request.Gebruiker.Username == username);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", localSettings.Values["token"].ToString());

            var json = JsonConvert.SerializeObject(new { Antwoord = false });

            HttpRequestMessage httpRequest = new HttpRequestMessage
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
                Method = HttpMethod.Delete,
                RequestUri = new Uri("http://localhost:58253/api/Requests/" + teVerwijderenRequest.Id)
            };

            var resp = await client.SendAsync(httpRequest);

            if (resp.StatusCode == HttpStatusCode.NoContent)
            {
                GetEigenWishlistsVanProfiel();
                Debug.WriteLine("Aanvraag geannuleerd");
            }
        }

        private async void VoegAanvraagToe(Wishlist wishlist)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            Gebruiker gebruiker = await GetIngelogdeGebruiker();

            Request request = new Request {Gebruiker = gebruiker, Wishlist = wishlist};

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", localSettings.Values["token"].ToString());
            var jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var resp = await client.PostAsync(new Uri("http://localhost:58253/api/Requests"), content);

            if (resp.IsSuccessStatusCode)
            {
               GetEigenWishlistsVanProfiel();
                Debug.WriteLine("Aanvraag verstuurd");
            }

        }

        private async Task<Gebruiker> GetIngelogdeGebruiker()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", localSettings.Values["token"].ToString());
            var json = await client.GetStringAsync(
                new Uri("http://localhost:58253/api/Gebruikers/ByUsername/" + localSettings.Values["username"]));

            return JsonConvert.DeserializeObject<Gebruiker>(json);

        }

        private async void AccepteerUitnodiging(Wishlist wishlist)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string username = localSettings.Values["username"].ToString();
            Uitnodiging teAccepterenUitnodiging = wishlist.VerzondenUitnodigingen
                .Find(uitnodiging => uitnodiging.Gebruiker.Username.Equals(username));

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", localSettings.Values["token"].ToString());

            var json = JsonConvert.SerializeObject(new { Antwoord = true });

            HttpRequestMessage httpRequest = new HttpRequestMessage
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
                Method = HttpMethod.Delete,
                RequestUri = new Uri("http://localhost:58253/api/Uitnodigingen/" + teAccepterenUitnodiging.Id)
            };

            var resp = await client.SendAsync(httpRequest);

            if (resp.StatusCode == HttpStatusCode.NoContent)
            {
                GetEigenWishlistsVanProfiel();
                Debug.WriteLine("Uitnodiging aanvaard");
            }
        }

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
