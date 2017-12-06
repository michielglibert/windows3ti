using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WishlistServices.Models
{
    public class WishlistDbContext : DbContext
    {
        public WishlistDbContext (DbContextOptions<WishlistDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Gebruiker> Gebruikers { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Wens> Wensen { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Uitnodiging> Uitnodigingen { get; set; }
        public DbSet<GekochtCadeau> GekochtCadeaus { get; set; }
    }
}
