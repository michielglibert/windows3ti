using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WishlistServices.Models
{
    public class Wishlist
    {
        public int Id { get; set; }

        public Gebruiker Ontvanger { get; set; }
        public List<Gebruiker> Kopers { get; set; }
        public List<Wens> Wensen { get; set; }
        public List<GekochtCadeau> GekochtCadeaus { get; set; }
        public List<Request> VerzondenUitnodigingen { get; set; }
        public List<Request> Requests { get; set; }

        public Wishlist()
        {
        }

        public void WensToevoegen(Wens wens)
        {
            throw new NotImplementedException();
        }

        public void WensVerwijderen(Wens wens)
        {
            throw new NotImplementedException();
        }

        public void UitnodigingToevoegen(Request uitnodiging)
        {
            throw new NotImplementedException();
        }

        public void RequestToevoegen(Request request)
        {
            throw new NotImplementedException();
        }

        public void MarkerenAlsGekocht(Wens wens)
        {
            throw new NotImplementedException();
        }

        public void WensWijzigen(Wens wens)
        {
            throw new NotImplementedException();
        }

    }
}
