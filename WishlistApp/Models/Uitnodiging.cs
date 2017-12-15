using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WishlistApp.Models;

namespace WishlisApp.Models
{
    public class Uitnodiging
    {
        public int Id { get; set; }
        public string Bericht { get; set; }

        public Gebruiker Gebruiker { get; set; }
        public Wishlist Wishlist { get; set; }

        public Uitnodiging(Wishlist wishlist, Gebruiker gebruiker)
        {
            this.Wishlist = wishlist;
            this.Gebruiker = gebruiker;
        }
    }
}
