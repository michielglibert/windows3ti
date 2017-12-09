using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WishlistServices.Data;
using WishlistServices.Models;

namespace WishlistServices.Controllers
{
    [Produces("application/json")]
    [Route("api/Wishlists")]
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly WishlistDbContext _context;

        public WishlistController(WishlistDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Alle wishlists opvragen
        /// </summary>
        // GET: api/Wishlists
        [HttpGet]
        public IEnumerable<Wishlist> GetWishlist()
        {
            return _context.Wishlists.Include(t => t.Ontvanger);
        }

        /// <summary>
        /// Wishlist zoeken
        /// </summary>
        [HttpGet]
        [Route("~/api/Wishlist/search")]
        public IEnumerable<Wishlist> GetWishlistBySearch([FromQuery] string naam)
        {
            if (naam.Length < 3)
            {
                throw new ArgumentException("Zoekopdracht moet langer zijn dan 3 tekens");
            }
            return _context.Wishlists.Where(t => t.Naam.Contains(naam));
        }

        /// <summary>
        /// Aantal cadeau's van wishlist per username (werkt nie deftig)
        /// </summary>
        [HttpGet]
        [Route("~/api/Wishlist/{wishlistId}/AantalGekochteCadeaus")]
        public Dictionary<Gebruiker, int> GetAantalGekochtCadeaus([FromRoute] int wishlistId)
        {
            var wishlist = _context.Wishlists
                .Include(t => t.Wensen).ThenInclude(t => t.GekochtCadeau).ThenInclude(t => t.Koper)
                .SingleOrDefault(t => t.Id == wishlistId);

            return wishlist.AantalGekochteCadeaus();
        }

        /// <summary>
        /// Wishlist by id
        /// </summary>
        // GET: api/Wishlists/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWishlist([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wishlist = await _context.Wishlists.Include(t => t.Ontvanger).SingleOrDefaultAsync(m => m.Id == id);
            if (wishlist == null)
            {
                return NotFound();
            }

            return Ok(wishlist);
        }

        /// <summary>
        /// Alle kopers van wishlist opvragen
        /// </summary>
        [HttpGet]
        [Route("~/api/Wishlists/{wishlistId}/Kopers")]
        public async Task<IActionResult> GetWishlistKopers([FromRoute] int wishlistId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wishlist = await _context.Wishlists
                .Include(t => t.Kopers).ThenInclude(t => t.Gebruiker)
                .SingleOrDefaultAsync(m => m.Id == wishlistId);
            if (wishlist == null)
            {
                return NotFound();
            }

            List<Gebruiker> kopers = new List<Gebruiker>();
            foreach (var koper in wishlist.Kopers)
            {
                kopers.Add(koper.Gebruiker);
            }

            return Ok(kopers);
        }

        /// <summary>
        /// Wishlist aanpassen
        /// </summary>
        // PUT: api/Wishlists/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWishlist([FromRoute] int id, [FromBody] Wishlist wishlist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != wishlist.Id)
            {
                return BadRequest();
            }

            _context.Entry(wishlist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WishlistExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Wishlist aanmaken
        /// </summary>
        // POST: api/Wishlists
        [HttpPost]
        public async Task<IActionResult> PostWishlist([FromBody] Wishlist wishlist)
        {
            var id = int.Parse(User.Claims.SingleOrDefault(t => t.Type == "id")?.Value);

            var gebruiker = _context.Gebruikers.SingleOrDefault(t => t.Id == id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            gebruiker.WishlistMaken(wishlist.Naam);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWishlist", new { id = wishlist.Id }, wishlist);
        }

        /// <summary>
        /// Wishlist verwijderen
        /// </summary>
        // DELETE: api/Wishlists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEigenWishlist([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wishlist = await _context.Wishlists.SingleOrDefaultAsync(m => m.Id == id);
            if (wishlist == null)
            {
                return NotFound();
            }

            _context.Wishlists.Remove(wishlist);
            await _context.SaveChangesAsync();

            return Ok(wishlist);
        }

        /// <summary>
        /// Wishlist verlaten
        /// </summary>
        // DELETE: api/Wishlists/5
        [HttpDelete]
        [Route("~/Wishlists/{wishlistId}/Verlaten")]
        public async Task<IActionResult> WishlistVerlaten([FromRoute] int wishlistId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = int.Parse(User.Claims.SingleOrDefault(t => t.Type == "id")?.Value);
            var gebruiker = _context.Gebruikers
                .Include(t => t.Wishlists).ThenInclude(t => t.Wishlist).ThenInclude(t => t.Kopers)
                .Include(t => t.Wishlists).ThenInclude(t => t.Gebruiker)
                .SingleOrDefault(t => t.Id == id);

            var wishlist = await _context.Wishlists.SingleOrDefaultAsync(m => m.Id == wishlistId);
            if (wishlist == null)
            {
                return NotFound();
            }
            
            gebruiker.WishlistVerlaten(wishlist);

            await _context.SaveChangesAsync();

            return Ok(wishlist);
        }

        private bool WishlistExists(int id)
        {
            return _context.Wishlists.Any(e => e.Id == id);
        }
    }
}
