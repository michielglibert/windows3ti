using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WishlistServices.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GebruikerWishlist",
                columns: table => new
                {
                    GebruikerId = table.Column<int>(type: "int", nullable: false),
                    WishlistId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GebruikerWishlist", x => new { x.GebruikerId, x.WishlistId });
                });

            migrationBuilder.CreateTable(
                name: "Request",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Bericht = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GebruikerId = table.Column<int>(type: "int", nullable: true),
                    WishlistId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Request", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Uitnodiging",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Bericht = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GebruikerId = table.Column<int>(type: "int", nullable: true),
                    WishlistId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uitnodiging", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wishlist",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naam = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    OntvangerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishlist", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GekochtCadeau",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Prijs = table.Column<double>(type: "float", nullable: false),
                    Wat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WishlistId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GekochtCadeau", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GekochtCadeau_Wishlist_WishlistId",
                        column: x => x.WishlistId,
                        principalTable: "Wishlist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Gebruiker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gebruiker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gebruiker_GekochtCadeau_Id",
                        column: x => x.Id,
                        principalTable: "GekochtCadeau",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Categorie = table.Column<int>(type: "int", nullable: false),
                    Gekocht = table.Column<bool>(type: "bit", nullable: false),
                    Omschrijving = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Titel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WishlistId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wens_GekochtCadeau_Id",
                        column: x => x.Id,
                        principalTable: "GekochtCadeau",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wens_Wishlist_WishlistId",
                        column: x => x.WishlistId,
                        principalTable: "Wishlist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GebruikerWishlist_WishlistId",
                table: "GebruikerWishlist",
                column: "WishlistId");

            migrationBuilder.CreateIndex(
                name: "IX_GekochtCadeau_WishlistId",
                table: "GekochtCadeau",
                column: "WishlistId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_GebruikerId",
                table: "Request",
                column: "GebruikerId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_WishlistId",
                table: "Request",
                column: "WishlistId");

            migrationBuilder.CreateIndex(
                name: "IX_Uitnodiging_GebruikerId",
                table: "Uitnodiging",
                column: "GebruikerId");

            migrationBuilder.CreateIndex(
                name: "IX_Uitnodiging_WishlistId",
                table: "Uitnodiging",
                column: "WishlistId");

            migrationBuilder.CreateIndex(
                name: "IX_Wens_WishlistId",
                table: "Wens",
                column: "WishlistId");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlist_OntvangerId",
                table: "Wishlist",
                column: "OntvangerId");

            migrationBuilder.AddForeignKey(
                name: "FK_GebruikerWishlist_Gebruiker_GebruikerId",
                table: "GebruikerWishlist",
                column: "GebruikerId",
                principalTable: "Gebruiker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GebruikerWishlist_Wishlist_WishlistId",
                table: "GebruikerWishlist",
                column: "WishlistId",
                principalTable: "Wishlist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Gebruiker_GebruikerId",
                table: "Request",
                column: "GebruikerId",
                principalTable: "Gebruiker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Wishlist_WishlistId",
                table: "Request",
                column: "WishlistId",
                principalTable: "Wishlist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Uitnodiging_Gebruiker_GebruikerId",
                table: "Uitnodiging",
                column: "GebruikerId",
                principalTable: "Gebruiker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Uitnodiging_Wishlist_WishlistId",
                table: "Uitnodiging",
                column: "WishlistId",
                principalTable: "Wishlist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlist_Gebruiker_OntvangerId",
                table: "Wishlist",
                column: "OntvangerId",
                principalTable: "Gebruiker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gebruiker_GekochtCadeau_Id",
                table: "Gebruiker");

            migrationBuilder.DropTable(
                name: "GebruikerWishlist");

            migrationBuilder.DropTable(
                name: "Request");

            migrationBuilder.DropTable(
                name: "Uitnodiging");

            migrationBuilder.DropTable(
                name: "Wens");

            migrationBuilder.DropTable(
                name: "GekochtCadeau");

            migrationBuilder.DropTable(
                name: "Wishlist");

            migrationBuilder.DropTable(
                name: "Gebruiker");
        }
    }
}
