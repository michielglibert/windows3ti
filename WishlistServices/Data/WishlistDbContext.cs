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
        
        public DbSet<Gebruiker> Gebruiker { get; set; }
        public DbSet<Wishlist> Type { get; set; }
        public DbSet<Wens> Wens { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<GekochtCadeau> GekochtCadeaus { get; set; }
    }
}
