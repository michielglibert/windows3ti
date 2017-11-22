using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WishlistServices.Models
{
    public class Wishlist
    {
        public Gebruiker Ontvanger { get; set; }
        public List<Gebruiker> Kopers { get; set; }
        public List<Wens> Wensen { get; set; }

    }
}
