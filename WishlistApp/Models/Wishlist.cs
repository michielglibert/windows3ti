using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WishlisApp.Models;
using WishlistApp.Data;

namespace WishlistApp.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        public string Naam { get; set; }

        public Gebruiker Ontvanger { get; set; }
        public List<GebruikerWishlist> Kopers { get; set; }
        public List<Wens> Wensen { get; set; }
        public List<Uitnodiging> VerzondenUitnodigingen { get; set; }
        public List<Request> Requests { get; set; }

    }
}
