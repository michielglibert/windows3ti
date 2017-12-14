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
                //Gebruikers
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


                //Wishlists
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
                wishlist2.Kopers.Add(new GebruikerWishlist(gebruiker5, wishlist2));
                wishlist2.Kopers.Add(new GebruikerWishlist(gebruiker6, wishlist2));
                wishlist2.Kopers.Add(new GebruikerWishlist(gebruiker7, wishlist2));
                wishlist2.Kopers.Add(new GebruikerWishlist(gebruiker8, wishlist2));
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

                //Init data
                foreach (var gebruiker in gebruikers)
                {
                    _dbContext.Gebruikers.Add(gebruiker);
                }

                foreach (var wishlist in wishlists)
                {
                    _dbContext.Wishlists.Add(wishlist);
                }

                //Uitnodigingen

                //Requests

                //Wensen





                //Save
                _dbContext.SaveChanges();
            }
        }
    }
}