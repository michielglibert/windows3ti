using System.Collections.Generic;
using WishlistServices.Models;

namespace WishlistServices.Data
{
    public class DummyDataInitializer
    {
        private readonly WishlistDbContext _dbContext;

        public DummyDataInitializer(WishlistDbContext context)
        {
            _dbContext = context;
        }

        public void InitData()
        {
            _dbContext.Database.EnsureDeleted();

            if (_dbContext.Database.EnsureCreated())
            {
                //Standaard login: Username(Jef), Password(Pass)
                //Standaard wishlist -> wishlist2

                #region Gebruikers
                var gebruikers = new List<Gebruiker>();

                var gebruiker1 = new Gebruiker("Giel","Pass");;
                gebruikers.Add(gebruiker1);

                var gebruiker2 = new Gebruiker("Karel","Pass");
                gebruikers.Add(gebruiker2);

                var gebruiker3 = new Gebruiker("Jef", "Pass");
                gebruikers.Add(gebruiker3);

                var gebruiker4 = new Gebruiker("Niels", "Pass");
                gebruikers.Add(gebruiker4);

                var gebruiker5 = new Gebruiker("Michiel", "Pass");
                gebruikers.Add(gebruiker5);

                var gebruiker6 = new Gebruiker("Tessa", "Pass");
                gebruikers.Add(gebruiker6);

                var gebruiker7 = new Gebruiker("Kaat", "Pass");
                gebruikers.Add(gebruiker7);

                var gebruiker8 = new Gebruiker("Kelly", "Pass");
                gebruikers.Add(gebruiker8);
                #endregion

                #region Wishlists
                var wishlists = new List<Wishlist>();

                var wishlist1 = new Wishlist("Bday bash") {Ontvanger = gebruiker1};
                wishlist1.Kopers.Add(new GebruikerWishlist(gebruiker2, wishlist1));
                wishlist1.Kopers.Add(new GebruikerWishlist(gebruiker3, wishlist1));
                wishlist1.Kopers.Add(new GebruikerWishlist(gebruiker4, wishlist1));
                wishlist1.Kopers.Add(new GebruikerWishlist(gebruiker5, wishlist1));
                wishlists.Add(wishlist1);

                var wishlist2 = new Wishlist("Kerst 2017") {Ontvanger = gebruiker3};
                wishlist2.Kopers.Add(new GebruikerWishlist(gebruiker2, wishlist2));
                wishlist2.Kopers.Add(new GebruikerWishlist(gebruiker1, wishlist2));
                wishlist2.Kopers.Add(new GebruikerWishlist(gebruiker4, wishlist2));
                wishlist2.Kopers.Add(new GebruikerWishlist(gebruiker6, wishlist2));
                wishlist2.Kopers.Add(new GebruikerWishlist(gebruiker7, wishlist2));
                wishlists.Add(wishlist2);

                var wishlist3 = new Wishlist("Nieuwjaar 2017") {Ontvanger = gebruiker3};
                wishlist3.Kopers.Add(new GebruikerWishlist(gebruiker2, wishlist3));
                wishlist3.Kopers.Add(new GebruikerWishlist(gebruiker4, wishlist3));
                wishlist3.Kopers.Add(new GebruikerWishlist(gebruiker5, wishlist3));
                wishlist3.Kopers.Add(new GebruikerWishlist(gebruiker6, wishlist3));
                wishlist3.Kopers.Add(new GebruikerWishlist(gebruiker7, wishlist3));
                wishlist3.Kopers.Add(new GebruikerWishlist(gebruiker8, wishlist3));
                wishlists.Add(wishlist3);

                var wishlist4 = new Wishlist("Verjaardagfeestje") {Ontvanger = gebruiker5};
                wishlist4.Kopers.Add(new GebruikerWishlist(gebruiker6, wishlist4));
                wishlist4.Kopers.Add(new GebruikerWishlist(gebruiker7, wishlist4));
                wishlist4.Kopers.Add(new GebruikerWishlist(gebruiker8, wishlist4));
                wishlists.Add(wishlist4);

                var wishlist5 = new Wishlist("Dingen die ik graag zou hebben") {Ontvanger = gebruiker7};
                wishlist5.Kopers.Add(new GebruikerWishlist(gebruiker4, wishlist5));
                wishlist5.Kopers.Add(new GebruikerWishlist(gebruiker5, wishlist5));
                wishlist5.Kopers.Add(new GebruikerWishlist(gebruiker6, wishlist5));
                wishlist5.Kopers.Add(new GebruikerWishlist(gebruiker8, wishlist5));
                wishlists.Add(wishlist5);

                var wishlist6 = new Wishlist("Dingen die ik graag zou hebben") { Ontvanger = gebruiker1 };
                wishlist6.Kopers.Add(new GebruikerWishlist(gebruiker2, wishlist6));
                wishlists.Add(wishlist6);
                #endregion

                #region Wensen

                var wensen = new List<Wens>();
                wishlist1.Wensen.Add(new Wens("Hololens", "Ik wil een hololens zodat ik men windows app kan testen"));
                wishlist1.Wensen.Add(new Wens("Appelsien", "Ik wil graag een verse appelsien"));
                wishlist1.Wensen.Add(new Wens("Holy Meme Bible", "Moe maar ke googlen kwil diene haha"));
                wishlist1.Wensen.Add(new Wens("Patatten", "Kwaliteit graag, kenners weten wel wat ik bedoel ;)"));

                wishlist2.Wensen.Add(new Wens("Hoedje van papier","Ik wil graag een hoedje van papier"));
                wishlist2.Wensen.Add(new Wens("Appel", "Ik wil graag een verse appel"));
                wishlist2.Wensen.Add(new Wens("Xbox One", "Een xbox voor mijn windows UWP apps op te testen"));
                wishlist2.Wensen.Add(new Wens("Nintendo Switch", "Nieuwe hype"));

                #endregion

                #region Uitnodigingen

                var uitnodigingen = new List<Uitnodiging>();
                Uitnodiging uitnodiging1 = new Uitnodiging(gebruiker8, wishlist2);
                wishlist2.VerzondenUitnodigingen.Add(uitnodiging1);
                gebruiker8.Uitnodigingen.Add(uitnodiging1);

                #endregion

                #region Requests

                var requests = new List<Request>();
                Request request1 = new Request(gebruiker5, wishlist2);
                wishlist2.Requests.Add(request1);
                gebruiker8.Requests.Add(request1);


                #endregion

                #region InitData
                foreach (var gebruiker in gebruikers)
                {
                    _dbContext.Gebruikers.Add(gebruiker);
                }

                foreach (var wishlist in wishlists)
                {
                    _dbContext.Wishlists.Add(wishlist);
                }

                foreach (var wens in wensen)
                {
                    _dbContext.Wensen.Add(wens);
                }

                foreach (var uitnodiging in uitnodigingen)
                {
                    _dbContext.Uitnodigingen.Add(uitnodiging);
                }

                foreach (var request in requests)
                {
                    _dbContext.Requests.Add(request);
                }
                
                _dbContext.SaveChanges();
                #endregion


            }
        }
    }
}