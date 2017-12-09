using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WishlistApp.Models
{
    public class Request
    {
        public int Id { get; set; }
        public string Bericht { get; set; }

        public Gebruiker Gebruiker { get; set; }
        public Wishlist Wishlist { get; set; }
        
    }
}
