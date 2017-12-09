namespace WishlistApp.Models
{
    public class GebruikerWishlist
    {
        public int GebruikerId { get; set; }
        public Gebruiker Gebruiker { get; set; }

        public int WishlistId { get; set; }
        public Wishlist Wishlist { get; set; }

        public GebruikerWishlist()
        {
            
        }

        public GebruikerWishlist(Gebruiker gebruiker, Wishlist wishlist)
        {
            GebruikerId = gebruiker.Id;
            Gebruiker = gebruiker;
            WishlistId = wishlist.Id;
            Wishlist = wishlist;
        }
    }
}
