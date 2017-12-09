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

        public ProfielViewModel()
        {
            //API CALLS
            EigenWishlists = new ObservableCollection<Wishlist>();
            OntvangenWishlists = new ObservableCollection<Wishlist>();
            Gebruiker = new Gebruiker{Naam = "Jef"};
        }
    }
}
