using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Threading.Tasks;
using WishlistServices.Data;

namespace WishlistServices.Models
{
    public class Gebruiker
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public List<Wishlist> EigenWishlists { get; set; }
        public List<GebruikerWishlist> Wishlists { get; set; }
        public List<Uitnodiging> Uitnodigingen { get; set; }
        public List<Request> Requests { get; set; }

        public Gebruiker()
        {
            EigenWishlists = new List<Wishlist>();
            Wishlists = new List<GebruikerWishlist>();
            Uitnodigingen = new List<Uitnodiging>();
            Requests = new List<Request>();
        }

        public Gebruiker(string username, string password):this()
        {
            Username = username;
            Password = password;
        }

        public Wishlist WishlistMaken(string naam)
        {
            Wishlist wishlist = new Wishlist(naam) {Ontvanger = this};
            EigenWishlists.Add(wishlist);
            return wishlist;

        }

        public void WishlistVerwijderen(Wishlist wishlist)
        {
            EigenWishlists.Remove(wishlist);
        }

        public void WishlistJoinen(Wishlist wishlist)
        {
            GebruikerWishlist gebruikerWishlist = new GebruikerWishlist(this, wishlist);
            Wishlists.Add(gebruikerWishlist);
            wishlist.KoperToevoegen(gebruikerWishlist);
        }

        public void WishlistVerlaten(Wishlist wishlist)
        {
            if (this == wishlist.Ontvanger)
            {
                throw new ArgumentException("Koper kan wishlist niet verlaten");
            }

            var teVerwijderenWishlist = Wishlists.SingleOrDefault(t => t.Wishlist == wishlist && t.Gebruiker == this);
            Wishlists.Remove(teVerwijderenWishlist);
            wishlist.KoperVerwijderen(this);
        }

        public void WensToevoegenAanWishlist(Wishlist wishlist, Wens wens)
        {
            wishlist.WensToevoegen(wens);
        }

        public void WensWijzigen(Wishlist wishlist, Wens wens)
        {
            wishlist.WensWijzigen(wens);
        }

        public void WensVerwijderenVanWishlist(Wishlist wishlist, Wens wens)
        {
            wishlist.WensVerwijderen(wens);
        }
        
        public void UitnodigingToevoegen(Uitnodiging uitnodiging)
        {
            Uitnodigingen.Add(uitnodiging);
        }

        public void UitnodigingVerwijderen(Uitnodiging uitnodiging)
        {
            Uitnodigingen.Remove(uitnodiging);
        }
        
        public void UitnodigenVoorWishlist(Gebruiker gebruiker, Wishlist wishlist)
        {
            wishlist.UitnodigingToevoegen(gebruiker);
        }

        public void RequestToevoegen(Request request)
        {
            Requests.Add(request);
        }

        public void RequestVerwijderen(Request request)
        {
            Requests.Remove(request);
        }

        public void RequestVersturenVoorWishlist(Wishlist wishlist)
        {
            wishlist.RequestToevoegen(this);
        }

        public void WensMarkerenAlsGekocht(Wishlist wishlist, Wens wens)
        {
            wishlist.WensMarkerenAlsGekocht(this, wens);
        }

    }
}
