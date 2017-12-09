using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WishlistApp.Models;

namespace WishlistApp.Data
{
    public class GebruikerWishlist
    {
        public int GebruikerId { get; set; }
        public Gebruiker Gebruiker { get; set; }

        public int WishlistId { get; set; }
        public Wishlist Wishlist { get; set; }

        public GebruikerWishlist()
        {
            
        }

        public GebruikerWishlist(Gebruiker gebruiker, Wishlist wishlist)
        {
            GebruikerId = gebruiker.Id;
            Gebruiker = gebruiker;
            WishlistId = wishlist.Id;
            Wishlist = wishlist;
        }
    }
}
