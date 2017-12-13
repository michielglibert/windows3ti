using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using WishlistApp.Models;

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

        public RelayCommand<Wishlist> JoinOrLeaveCommand { get; set; }
        public RelayCommand<Wishlist> NodigUitCommand { get; set; }

        public ProfielViewModel()
        {
            //TODO: API CALLS
            EigenWishlists = new ObservableCollection<Wishlist>(GenerateEigenWishlists());
            OntvangenWishlists = new ObservableCollection<Wishlist>(GenerateAndereWishlists());
            WishlistsIngelogdeGebruiker = new ObservableCollection<Wishlist>(GenerateWishlistsIngelogdeGebruiker());
            Gebruiker = new Gebruiker{Username = "Koen"};

            JoinOrLeaveCommand = new RelayCommand<Wishlist>(JoinOrLeaveWishlist);
            NodigUitCommand = new RelayCommand<Wishlist>(NodigUit);
        }

        private void NodigUit(Wishlist wishlist)
        {
            //TODO: NodigUit() implementeren
            //wishlist.NodigUit();
            System.Diagnostics.Debug.WriteLine("NodigUit werkt");
            System.Diagnostics.Debug.WriteLine(wishlist.Naam);
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

        public List<Wishlist> GenerateAndereWishlists()
        {
            Wishlist w1 = new Wishlist { Naam = "Fatima's Hanoeka Wishlist", Ontvanger = new Gebruiker {Username = "Fatima"}};
            Wishlist w2 = new Wishlist { Naam = "Bob's vrijgezellenfeest Wensenlijst", Ontvanger = new Gebruiker { Username = "Bob" } };
            Wishlist w3 = new Wishlist { Naam = "Thomas's uit de kast gekomen Wensenlijst", Ontvanger = new Gebruiker { Username = "Thomas" } };

            List<Wishlist> wishlists = new List<Wishlist> { w1, w2, w3 };
            return wishlists;

        }

        public List<Wishlist> GenerateWishlistsIngelogdeGebruiker()
        {
            Wishlist w1 = new Wishlist { Naam = "Verjaardag Jef", Ontvanger = new Gebruiker{Username = "Jef"} };

            List<Wishlist> wishlists = new List<Wishlist> { w1};
            return wishlists;

        }
    }
}
