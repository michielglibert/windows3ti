using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WishlistApp.Models;

namespace WishlistApp.Viewmodels
{
    public class ProfielViewModel : ViewModelBase
    {
        public ObservableCollection<Wishlist> EigenWishlists { get; set; }
        public ObservableCollection<Wishlist> OntvangenWishlists { get; set; }
        public Gebruiker Gebruiker { get; set; }
        public string ButtonText { get; set; }

        public ProfielViewModel()
        {
            //API CALLS
            EigenWishlists = new ObservableCollection<Wishlist>(generateWishlist());
            OntvangenWishlists = new ObservableCollection<Wishlist>();
            Gebruiker = new Gebruiker{Naam = "Jef"};
        }

        public List<Wishlist> generateWishlist()
        {
            Wishlist w1 = new Wishlist {Naam = "Wishlist 1"};
            Wishlist w2 = new Wishlist {Naam = "Wishlist 2"};

            List<Wishlist> wishlists = new List<Wishlist> {w1, w2};
            return wishlists;

        }
    }
}
