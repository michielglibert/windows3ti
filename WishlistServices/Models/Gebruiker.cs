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
        public List<Request> Uitnodigingen { get; set; }
        public List<Request> Requests { get; set; }

        public Gebruiker()
        {
        }

        public void WishlistMaken()
        {
            throw new NotImplementedException();
        }

        public void WishlistVerwijderen(Wishlist wishlist)
        {
            throw new NotImplementedException();
        }

        public void WensToevoegenAanWishlist(Wishlist wishlist, Wens wens)
        {
            throw new NotImplementedException();
        }

        public void WensWijzigen(Wishlist wishlist, Wens wens)
        {
            throw new NotImplementedException();
        }

        public void WensVerwijderenVanWishlist(Wishlist wishlist, Wens wens)
        {
            throw new NotImplementedException();
        }

        public void MarkerenAlsGekocht(Wishlist wishlist, Wens wens, GekochtCadeau gekochtCadeau)
        {
            throw new NotImplementedException();
        }

        public void UitnodigingAccepteren(Request uitnodiging)
        {
            throw new NotImplementedException();
        }

        public void UitnodigingAfwijzen(Request uitnodiging)
        {
            throw new NotImplementedException();
        }

        public void UitnodigenVoorWishlist(Gebruiker gebruiker, Wishlist wishlist)
        {
            throw new NotImplementedException();
        }

        public void RequestAccepteren(Request request)
        {
            throw new NotImplementedException();
        }

        public void RequestAfwijzen(Request request)
        {
            throw new NotImplementedException();
        }

        public void RequestVersturenVoorWishlist(Wishlist wishlist)
        {
            throw new NotImplementedException();
        }

    }
}
