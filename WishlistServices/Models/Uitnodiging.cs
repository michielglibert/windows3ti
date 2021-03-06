﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WishlistServices.Models
{
    public class Uitnodiging
    {
        public int Id { get; set; }
        public string Bericht { get; set; }

        public Gebruiker Gebruiker { get; set; }
        public Wishlist Wishlist { get; set; }

        public Uitnodiging()
        {
        }

        public Uitnodiging(Gebruiker gebruiker, Wishlist wishlist)
        {
            Gebruiker = gebruiker;
            Wishlist = wishlist;
        }

        public void AccepteerUitnodiging()
        {
            Gebruiker.WishlistJoinen(Wishlist);
            WijsUitnodigingAf();
        }

        public void WijsUitnodigingAf()
        {
            Gebruiker.UitnodigingVerwijderen(this);
            Wishlist.UitnodigingVerwijderen(this);
        }
    }
}
