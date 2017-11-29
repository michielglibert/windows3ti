using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WishlistServices.Models
{
    public class Request
    {
        public int Id { get; set; }
        public string Bericht { get; set; }

        public Gebruiker Gebruiker { get; set; }
        public Wishlist Wishlist { get; set; }

        public Request()
        {
        }

        public void AccepteerRequest()
        {
            
        }

        public void WijsRequestAf()
        {
            
        }
    }
}
