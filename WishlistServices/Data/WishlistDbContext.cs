using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WishlistServices.Data;

namespace WishlistServices.Models
{
    public class WishlistDbContext : DbContext
    {
        public WishlistDbContext (DbContextOptions<WishlistDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Gebruiker>(MapGebruiker);
            builder.Entity<Wishlist>(MapWishlist);
            builder.Entity<Wens>(MapWens);
            builder.Entity<Request>(MapRequest);
            builder.Entity<Uitnodiging>(MapUitnodiging);
            builder.Entity<GekochtCadeau>(MapGekochtCadeau);
            builder.Entity<GebruikerWishlist>(MapGebruikerWishlist);
        }

        private void MapGebruikerWishlist(EntityTypeBuilder<GebruikerWishlist> gw)
        {
            //Table
            gw.ToTable("GebruikerWishlist");
            gw.HasKey(t => new { t.GebruikerId, t.WishlistId });

            gw.HasOne(t => t.Wishlist)
                .WithMany(t => t.Kopers);

            gw.HasOne(t => t.Gebruiker)
                .WithMany(t => t.Wishlists);
        }

        private void MapGekochtCadeau(EntityTypeBuilder<GekochtCadeau> gc)
        {
            //Table
            gc.ToTable("GekochtCadeau");
            gc.HasKey(t => t.Id);

            //Relations
            gc.HasOne(t => t.Koper)
                .WithOne()
                .HasForeignKey<Gebruiker>(t => t.Id);

            //Props
            gc.Property(t => t.Wat)
                .HasColumnName("Wat")
                .IsRequired();

            gc.Property(t => t.Prijs)
                .HasColumnName("Prijs")
                .IsRequired();
        }

        private void MapUitnodiging(EntityTypeBuilder<Uitnodiging> u)
        {
            //Table
            u.ToTable("Uitnodiging");
            u.HasKey(t => t.Id);

            //Relations
            u.HasOne(t => t.Wishlist)
                .WithMany(t => t.VerzondenUitnodigingen);
            u.HasOne(t => t.Gebruiker)
                .WithMany(t => t.Uitnodigingen);

            //Props
            u.Property(t => t.Bericht)
                .HasColumnName("Bericht");
        }

        private void MapRequest(EntityTypeBuilder<Request> r)
        {
            //Table
            r.ToTable("Request");
            r.HasKey(t => t.Id);

            //Relations
            r.HasOne(t => t.Wishlist)
                .WithMany(t => t.Requests);
            r.HasOne(t => t.Gebruiker)
                .WithMany(t => t.Requests);

            //Props
            r.Property(t => t.Bericht)
                .HasColumnName("Bericht");
        }

        private void MapWens(EntityTypeBuilder<Wens> w)
        {
            //Table
            w.ToTable("Wens");
            w.HasKey(t => t.Id);

            //Relations
            w.HasOne(t => t.GekochtCadeau)
                .WithOne()
                .HasForeignKey<Wens>(t => t.Id); ;

            //Props
            w.Property(t => t.Categorie)
                .HasColumnName("Categorie")
                .IsRequired();
            w.Property(t => t.Gekocht)
                .HasColumnName("Gekocht");
            w.Property(t => t.Titel)
                .HasColumnName("Titel")
                .IsRequired();
            w.Property(t => t.Omschrijving)
                .HasColumnName("Omschrijving");
            w.Ignore(t => t.Foto);
        }

        private void MapWishlist(EntityTypeBuilder<Wishlist> w)
        {
            //Table
            w.ToTable("Wishlist");
            w.HasKey(t => t.Id);

            //Relations
            w.HasMany(t => t.Wensen)
                .WithOne();
            w.HasMany(t => t.GekochtCadeaus)
                .WithOne();
            w.HasMany(t => t.Requests)
                .WithOne(t => t.Wishlist);
            w.HasMany(t => t.VerzondenUitnodigingen)
                .WithOne(t => t.Wishlist);
            w.HasMany(t => t.Kopers)
                .WithOne(t => t.Wishlist);
            w.HasOne(t => t.Ontvanger)
                .WithMany(t => t.EigenWishlists)
                .OnDelete(DeleteBehavior.Restrict);

            //Props
            w.Property(t => t.Naam)
                .HasColumnName("Naam")
                .HasMaxLength(250)
                .IsRequired();
        }

        private void MapGebruiker(EntityTypeBuilder<Gebruiker> g)
        {
            //Table
            g.ToTable("Gebruiker");
            g.HasKey(t => t.Id);

            //Relations
            g.HasMany(t => t.Requests)
                .WithOne(t => t.Gebruiker);
            g.HasMany(t => t.Uitnodigingen)
                .WithOne(t => t.Gebruiker);
            g.HasMany(t => t.Wishlists)
                .WithOne(t => t.Gebruiker);
            g.HasMany(t => t.EigenWishlists)
                .WithOne(t => t.Ontvanger)
                .IsRequired();

            //Props
            g.Property(t => t.Username)
                .HasColumnName("Username")
                .HasMaxLength(35)
                .IsRequired();
        }


        public DbSet<Gebruiker> Gebruikers { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Wens> Wensen { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Uitnodiging> Uitnodigingen { get; set; }
        public DbSet<GekochtCadeau> GekochtCadeaus { get; set; }
    }
}
