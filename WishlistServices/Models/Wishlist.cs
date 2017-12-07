using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using WishlistServices.Data;

namespace WishlistServices.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        public string Naam { get; set; }

        public Gebruiker Ontvanger { get; set; }
        public List<GebruikerWishlist> Kopers { get; set; }
        public List<Wens> Wensen { get; set; }
        public List<GekochtCadeau> GekochtCadeaus { get; set; }
        public List<Uitnodiging> VerzondenUitnodigingen { get; set; }
        public List<Request> Requests { get; set; }

        public Wishlist()
        {
            Kopers = new List<GebruikerWishlist>();
            Wensen = new List<Wens>();
            GekochtCadeaus = new List<GekochtCadeau>();
            VerzondenUitnodigingen = new List<Uitnodiging>();
            Requests = new List<Request>();
        }

        public Wishlist(string naam)
        {
            Naam = naam;
        }

        public void KoperToevoegen(Gebruiker gebruiker)
        {
            Kopers.Add(new GebruikerWishlist(gebruiker, this));
        }

        public void KoperVerwijderen(Gebruiker gebruiker)
        {
            var teVerwijderenGebruiker = Kopers.SingleOrDefault(t => t.Wishlist == this && t.Gebruiker == gebruiker);
            Kopers.Remove(teVerwijderenGebruiker);
        }

        public void WensToevoegen(Wens wens)
        {
            Wensen.Add(wens);
        }

        public void WensVerwijderen(Wens wens)
        {
            Wensen.Remove(wens);
        }

        public void UitnodigingToevoegen(Gebruiker gebruiker)
        {
            VerzondenUitnodigingen.Add(new Uitnodiging(gebruiker, this));
        }
        public void UitnodigingVerwijderen(Uitnodiging uitnodiging)
        {
            VerzondenUitnodigingen.Remove(uitnodiging);
        }

        public void RequestToevoegen(Gebruiker gebruiker)
        {
            Requests.Add(new Request(gebruiker, this));
        }

        public void RequestVerwijderen(Request request)
        {
            Requests.Remove(request);
        }

        public void MarkerenAlsGekocht(Wens wens, GekochtCadeau gekochtCadeau)
        {
            wens.GekochtCadeau = gekochtCadeau;
        }

        public void WensWijzigen(Wens wens)
        {
            var index = Wensen.IndexOf(wens);

            try
            {
                Wensen[index] = wens;
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Wens niet gevonden");
            }
        }

    }
}
