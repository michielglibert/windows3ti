using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Threading.Tasks;

namespace WishlistServices.Models
{
    public class Gebruiker
    {
        public int Id { get; set; }
        public string Naam { get; set; }

        public List<Wishlist> EigenWishlists { get; set; }
        public List<Wishlist> Wishlists { get; set; }
        public List<Uitnodiging> Uitnodigingen { get; set; }
        public List<Request> Requests { get; set; }

        public Gebruiker()
        {
        }

        public void WishlistMaken()
        {
            Wishlist wishlist = new Wishlist();
            wishlist.Ontvanger = this;
            EigenWishlists.Add(wishlist);
            
        }

        public void WishlistVerwijderen(Wishlist wishlist)
        {
            EigenWishlists.Remove(wishlist);
        }

        public void WishlistJoinen(Wishlist wishlist)
        {
            Wishlists.Add(wishlist);
            wishlist.KoperToevoegen(this);
        }

        public void WishlistVerlaten(Wishlist wishlist)
        {
            Wishlists.Add(wishlist);
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

        public void MarkerenAlsGekocht(Wishlist wishlist, Wens wens, GekochtCadeau gekochtCadeau)
        {
            wishlist.MarkerenAlsGekocht(wens, gekochtCadeau);
        }

        public void UitnodigingToevoegen(Uitnodiging uitnodiging)
        {
            Uitnodigingen.Add(uitnodiging);
        }

        public void UitnodigingVerwijderen(Uitnodiging uitnodiging)
        {
            Uitnodigingen.Remove(uitnodiging);
        }

        public void UitnodigingAccepteren(Request uitnodiging)
        {
            uitnodiging.AccepteerRequest();
        }

        public void UitnodigingAfwijzen(Request uitnodiging)
        {
            uitnodiging.WijsRequestAf();
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

        public void RequestAccepteren(Request request)
        {
            request.AccepteerRequest();
        }

        public void RequestAfwijzen(Request request)
        {
            request.WijsRequestAf();
        }

        public void RequestVersturenVoorWishlist(Wishlist wishlist)
        {
            wishlist.RequestToevoegen(this);
        }

    }
}
