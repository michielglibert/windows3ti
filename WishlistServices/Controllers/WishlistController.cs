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

        // GET: api/Wishlists
        [HttpGet]
        public IEnumerable<Wishlist> GetWishlist()
        {
            return _context.Wishlists.Include(t => t.Ontvanger);
        }

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

        // DELETE: api/Wishlists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWishlist([FromRoute] int id)
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

        private bool WishlistExists(int id)
        {
            return _context.Wishlists.Any(e => e.Id == id);
        }
    }
}
