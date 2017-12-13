using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WishlistApp.Models;
using WishlistApp.Utils;

namespace WishlistApp.Viewmodels
{
    public class ZoekViewModel: ViewModelBase
    {
        public ObservableCollection<Gebruiker> GebruikersLijst { get; set; }
        public IEnumerable<Gebruiker> ResultGebruikersLijst { get; set; }
        public ObservableCollection<Wishlist> WishlistLijst { get; set; }
        public IEnumerable<Wishlist> ResultWishlistLijst { get; set; }
    
        public List<string> ZoekSoorten { get; set; }
        public string ZoekString { get; set; }
        public string GeselecteerdeSoort { get; set; }
        public RelayCommand ZoekCommand { get; set; }

        public ZoekViewModel()
        {
            ZoekSoorten = new List<string>{"Gebruiker", "Wishlist"};
            GebruikersLijst = new ObservableCollection<Gebruiker>(GenerateGebruikersList());
            WishlistLijst = new ObservableCollection<Wishlist>(GenerateWishlists());
            ZoekCommand = new RelayCommand((param) => StelLijstIn(param));
        }

        private void StelLijstIn(object zoekString)
        {
            Debug.WriteLine(zoekString);
            Debug.WriteLine(GeselecteerdeSoort);
            Debug.WriteLine("ZoekButton geklikt");

            if (GeselecteerdeSoort.Equals("Gebruiker"))
            {
               GebruikersLijst = new ObservableCollection<Gebruiker>(ZoekGebruikersMetNaam(zoekString.ToString()));
            }
            else
            {
                WishlistLijst = new ObservableCollection<Wishlist>(GenerateWishlists());
            }
            Debug.WriteLine(GebruikersLijst);
        }


        public List<Gebruiker> GenerateGebruikersList()
        {
            Gebruiker g1 = new Gebruiker { Naam = "Frank" };
            Gebruiker g2 = new Gebruiker { Naam = "Henk" };
            Gebruiker g3 = new Gebruiker { Naam = "Mariette" };
            Gebruiker g4 = new Gebruiker { Naam = "Jean-Claude" };
            Gebruiker g5 = new Gebruiker { Naam = "Wilhelm" };

            List<Gebruiker> gebruikers = new List<Gebruiker>{g1, g2, g3, g4, g5};
            return gebruikers;
        }

        private List<Wishlist> GenerateWishlists()
        {
            Wishlist w1 = new Wishlist { Naam = "Fatima's Hanoeka Wishlist", Ontvanger = new Gebruiker { Naam = "Fatima" } };
            Wishlist w2 = new Wishlist { Naam = "Bob's vrijgezellenfeest Wensenlijst", Ontvanger = new Gebruiker { Naam = "Bob" } };
            Wishlist w3 = new Wishlist { Naam = "Thomas's uit de kast gekomen Wensenlijst", Ontvanger = new Gebruiker { Naam = "Thomas" } };

            List<Wishlist> wishlists = new List<Wishlist> { w1, w2, w3 };
            return wishlists;
        }

        public IEnumerable<Gebruiker> ZoekGebruikersMetNaam(string zoekString)
        {
            return GebruikersLijst.Where(g => g.Naam == ZoekString);
        }
    }
}
