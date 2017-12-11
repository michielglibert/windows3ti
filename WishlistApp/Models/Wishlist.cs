using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WishlisApp.Models;

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

        public bool IsHuidigeGebruikerDeelVanWishlist()
        {
            //TODO: ophalen van huidige gebruiker
            //TODO: is huidige gebruiker deel van wishlist?
            return true;
        }

        public string JoinOrLeaveText
        {
            get
            {
                if (IsHuidigeGebruikerDeelVanWishlist())
                {
                    return "Neem deel";
                }
                else
                {
                    return "Verlaat wishlist";
                }
            }
        }
    }
}
