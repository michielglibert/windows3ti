using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
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

        public Request(Gebruiker gebruiker, Wishlist wishlist)
        {
            Gebruiker = gebruiker;
            Wishlist = wishlist;
        }

        public void AccepteerRequest()
        {
            Gebruiker.WishlistJoinen(Wishlist);
        }

        public void WijsRequestAf()
        {
            Gebruiker.RequestVerwijderen(this);
            Wishlist.RequestVerwijderen(this);
        }


    }
}
