﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using WishlistServices.Data;
using WishlistServices.Models;

namespace WishlistServices.Migrations
{
    [DbContext(typeof(WishlistDbContext))]
    [Migration("20171206225854_AddPasswordField")]
    partial class AddPasswordField
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WishlistServices.Data.GebruikerWishlist", b =>
                {
                    b.Property<int>("GebruikerId");

                    b.Property<int>("WishlistId");

                    b.HasKey("GebruikerId", "WishlistId");

                    b.HasIndex("WishlistId");

                    b.ToTable("GebruikerWishlist");
                });

            modelBuilder.Entity("WishlistServices.Models.Gebruiker", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Password");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnName("Username")
                        .HasMaxLength(35);

                    b.HasKey("Id");

                    b.ToTable("Gebruiker");
                });

            modelBuilder.Entity("WishlistServices.Models.GekochtCadeau", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Prijs")
                        .HasColumnName("Prijs");

                    b.Property<string>("Wat")
                        .IsRequired()
                        .HasColumnName("Wat");

                    b.Property<int?>("WishlistId");

                    b.HasKey("Id");

                    b.HasIndex("WishlistId");

                    b.ToTable("GekochtCadeau");
                });

            modelBuilder.Entity("WishlistServices.Models.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bericht")
                        .HasColumnName("Bericht");

                    b.Property<int?>("GebruikerId");

                    b.Property<int?>("WishlistId");

                    b.HasKey("Id");

                    b.HasIndex("GebruikerId");

                    b.HasIndex("WishlistId");

                    b.ToTable("Request");
                });

            modelBuilder.Entity("WishlistServices.Models.Uitnodiging", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bericht")
                        .HasColumnName("Bericht");

                    b.Property<int?>("GebruikerId");

                    b.Property<int?>("WishlistId");

                    b.HasKey("Id");

                    b.HasIndex("GebruikerId");

                    b.HasIndex("WishlistId");

                    b.ToTable("Uitnodiging");
                });

            modelBuilder.Entity("WishlistServices.Models.Wens", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int>("Categorie")
                        .HasColumnName("Categorie");

                    b.Property<bool>("Gekocht")
                        .HasColumnName("Gekocht");

                    b.Property<string>("Omschrijving")
                        .HasColumnName("Omschrijving");

                    b.Property<string>("Titel")
                        .IsRequired()
                        .HasColumnName("Titel");

                    b.Property<int?>("WishlistId");

                    b.HasKey("Id");

                    b.HasIndex("WishlistId");

                    b.ToTable("Wens");
                });

            modelBuilder.Entity("WishlistServices.Models.Wishlist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Naam")
                        .IsRequired()
                        .HasColumnName("Naam")
                        .HasMaxLength(250);

                    b.Property<int?>("OntvangerId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("OntvangerId");

                    b.ToTable("Wishlist");
                });

            modelBuilder.Entity("WishlistServices.Data.GebruikerWishlist", b =>
                {
                    b.HasOne("WishlistServices.Models.Gebruiker", "Gebruiker")
                        .WithMany("Wishlists")
                        .HasForeignKey("GebruikerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WishlistServices.Models.Wishlist", "Wishlist")
                        .WithMany("Kopers")
                        .HasForeignKey("WishlistId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WishlistServices.Models.Gebruiker", b =>
                {
                    b.HasOne("WishlistServices.Models.GekochtCadeau")
                        .WithOne("Koper")
                        .HasForeignKey("WishlistServices.Models.Gebruiker", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WishlistServices.Models.GekochtCadeau", b =>
                {
                    b.HasOne("WishlistServices.Models.Wishlist")
                        .WithMany("GekochtCadeaus")
                        .HasForeignKey("WishlistId");
                });

            modelBuilder.Entity("WishlistServices.Models.Request", b =>
                {
                    b.HasOne("WishlistServices.Models.Gebruiker", "Gebruiker")
                        .WithMany("Requests")
                        .HasForeignKey("GebruikerId");

                    b.HasOne("WishlistServices.Models.Wishlist", "Wishlist")
                        .WithMany("Requests")
                        .HasForeignKey("WishlistId");
                });

            modelBuilder.Entity("WishlistServices.Models.Uitnodiging", b =>
                {
                    b.HasOne("WishlistServices.Models.Gebruiker", "Gebruiker")
                        .WithMany("Uitnodigingen")
                        .HasForeignKey("GebruikerId");

                    b.HasOne("WishlistServices.Models.Wishlist", "Wishlist")
                        .WithMany("VerzondenUitnodigingen")
                        .HasForeignKey("WishlistId");
                });

            modelBuilder.Entity("WishlistServices.Models.Wens", b =>
                {
                    b.HasOne("WishlistServices.Models.GekochtCadeau", "GekochtCadeau")
                        .WithOne()
                        .HasForeignKey("WishlistServices.Models.Wens", "Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WishlistServices.Models.Wishlist")
                        .WithMany("Wensen")
                        .HasForeignKey("WishlistId");
                });

            modelBuilder.Entity("WishlistServices.Models.Wishlist", b =>
                {
                    b.HasOne("WishlistServices.Models.Gebruiker", "Ontvanger")
                        .WithMany("EigenWishlists")
                        .HasForeignKey("OntvangerId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
