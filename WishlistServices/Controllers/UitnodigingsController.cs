using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WishlistServices.Data;
using WishlistServices.Models;

namespace WishlistServices.Controllers
{
    [Produces("application/json")]
    [Route("api/Uitnodigingen")]
    [Authorize]
    public class UitnodigingsController : Controller
    {
        private readonly WishlistDbContext _context;

        public UitnodigingsController(WishlistDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Alle uitnodigingen van gebruiker bekijken
        /// </summary>
        // GET: api/Uitnodigingen
        [HttpGet]
        public IEnumerable<Uitnodiging> GetUitnodigingenVanGebruiker()
        {
            var id = int.Parse(User.Claims.SingleOrDefault(t => t.Type == "id")?.Value);
            var gebruiker = _context.Gebruikers
                .Include(t => t.Uitnodigingen).ThenInclude(t => t.Wishlist).ThenInclude(t => t.Ontvanger)
                .Include(t => t.Uitnodigingen).ThenInclude(t => t.Wishlist).ThenInclude(t => t.Wensen)
                .Include(t => t.Uitnodigingen).ThenInclude(t => t.Wishlist).ThenInclude(t => t.Kopers)
                .SingleOrDefault(t => t.Id == id);

            return gebruiker.Uitnodigingen;
        }

        [HttpGet]
        [Route("~/api/Wishlists/{wishlistId}/Uitnodigingen")]
        public IEnumerable<Uitnodiging> GetUitnodigingenVanWishlist([FromRoute] int wishlistId)
        {
            var wishlist = _context.Wishlists
                .Include(t => t.VerzondenUitnodigingen).ThenInclude(t => t.Gebruiker)
                .SingleOrDefault(t => t.Id == wishlistId);

            return wishlist.VerzondenUitnodigingen;
        }
        
        /// <summary>
        /// Uitnodiging by id
        /// </summary>
        // GET: api/Uitnodigingen/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUitnodiging([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var uitnodiging = await _context.Uitnodigingen.SingleOrDefaultAsync(m => m.Id == id);

            if (uitnodiging == null)
            {
                return NotFound();
            }

            return Ok(uitnodiging);
        }

        /// <summary>
        /// Uitnodiging aanvaard/afwijzen: { "antwoord":true/false }
        /// </summary>
        [HttpDelete]
        [Route("~/api/Uitnodigingen/{uitnodigingId}")]
        public IActionResult RequestBeantwoorden([FromRoute] int uitnodigingId, [FromBody] InviteRequest antwoord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var uitnodiging = _context.Uitnodigingen
                .Include(t => t.Gebruiker).ThenInclude(t => t.Uitnodigingen)
                .Include(t => t.Wishlist).ThenInclude(t => t.VerzondenUitnodigingen)
                .SingleOrDefault(t => t.Id == uitnodigingId);

            if (uitnodiging == null)
            {
                return NotFound();
            }

            if (antwoord.Antwoord)
            {
                uitnodiging.AccepteerUitnodiging();
            }
            else
            {
                uitnodiging.WijsUitnodigingAf();
            }

            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Uitnodiging toevoegen
        /// </summary>
        // POST: api/Uitnodigingen
        [HttpPost]
        [Route("~/api/Uitnodigingen")]
        public async Task<IActionResult> PostUitnodiging([FromBody] Uitnodiging uitnodiging)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = int.Parse(User.Claims.SingleOrDefault(t => t.Type == "id")?.Value);
            var gebruiker = _context.Gebruikers.SingleOrDefault(t => t.Id == id);

            var wishlist = _context.Wishlists.SingleOrDefault(t => t.Id == uitnodiging.Wishlist.Id);
            var uitgenodigdeGebruiker = _context.Gebruikers.SingleOrDefault(t => t.Id == uitnodiging.Gebruiker.Id);

            gebruiker.UitnodigenVoorWishlist(uitgenodigdeGebruiker, wishlist);
            
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUitnodiging", new { id = uitnodiging.Id }, uitnodiging);
        }
        
    }
}

public class InviteRequest
{
    public bool Antwoord { set; get; }
}
