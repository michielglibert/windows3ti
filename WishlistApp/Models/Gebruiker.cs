using System.Collections.Generic;
using System.Linq;
using WishlisApp.Models;

namespace WishlistApp.Models
{
    public class Gebruiker
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public List<Wishlist> EigenWishlists { get; set; }
        public List<GebruikerWishlist> Wishlists { get; set; }
        public List<Uitnodiging> Uitnodigingen { get; set; }
        public List<Request> Requests { get; set; }

        public int GetAantalGekochtVanWishlist(Wishlist wishlist)
        {
            return wishlist.Wensen.Count(w => w.GekochtCadeau.Koper == this);
        }
    }


}