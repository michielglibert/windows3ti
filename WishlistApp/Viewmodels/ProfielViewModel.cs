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
            get { return Gebruiker.Naam; }
        }

        public RelayCommand<Wishlist> JoinOrLeaveCommand { get; set; }


        public ProfielViewModel()
        {
            //TODO: API CALLS
            EigenWishlists = new ObservableCollection<Wishlist>(GenerateEigenWishlists());
            OntvangenWishlists = new ObservableCollection<Wishlist>(GenerateAndereWishlists());
            Gebruiker = new Gebruiker{Naam = "Jef"};
            JoinOrLeaveCommand = new RelayCommand<Wishlist>(JoinOrLeaveWishlist);
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
            Wishlist w1 = new Wishlist { Naam = "Fatima's Hanoeka Wishlist", Ontvanger = new Gebruiker {Naam = "Fatima"}};
            Wishlist w2 = new Wishlist { Naam = "Bob's vrijgezellenfeest Wensenlijst", Ontvanger = new Gebruiker { Naam = "Bob" } };
            Wishlist w3 = new Wishlist { Naam = "Thomas's uit de kast gekomen Wensenlijst", Ontvanger = new Gebruiker { Naam = "Thomas" } };

            List<Wishlist> wishlists = new List<Wishlist> { w1, w2, w3 };
            return wishlists;

        }

    }
}
