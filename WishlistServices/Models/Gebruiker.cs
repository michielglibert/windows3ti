using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WishlistServices.Models
{
    public class Gebruiker
    {
        public List<Wishlist> EigenWishlists { get; set; }
        public List<Wishlist> Wishlists { get; set; }
        public string Naam { get; set; }
    }
}
