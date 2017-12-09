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
    [Route("api/Wensen")]
    [Authorize]
    public class WensController : Controller
    {
        private readonly WishlistDbContext _context;

        public WensController(WishlistDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Alle wensen van wishlist opvragen
        /// </summary>
        [HttpGet]
        [Route("~/api/Wishlist/{wishlistId}/Wensen")]
        public IEnumerable<Wens> GetWensenVanWishlist([FromRoute] int wishlistId)
        {
            var wishlist = _context.Wishlists
                .Include(t => t.Wensen)
                .ThenInclude(t => t.GekochtCadeau)
                .SingleOrDefault(t => t.Id == wishlistId);
            
            return wishlist.Wensen;
        }

        /// <summary>
        /// Alle mogelijke categorien voor wishlist opvragen => string[]
        /// </summary>
        [HttpGet]
        [Route("~/api/Wensen/Categorien")]
        public IEnumerable<string> GetCategorien()
        {
            return Enum.GetNames(typeof(Categorie)).ToList();
        }

        /// <summary>
        /// Wens by id
        /// </summary>
        // GET: api/Wens/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWens([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wens = await _context.Wensen.SingleOrDefaultAsync(m => m.Id == id);

            if (wens == null)
            {
                return NotFound();
            }

            return Ok(wens);
        }

        /// <summary>
        /// Wens aanpassen
        /// </summary>
        // PUT: api/Wensen/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWens([FromRoute] int id, [FromBody] Wens wens)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != wens.Id)
            {
                return BadRequest();
            }

            _context.Entry(wens).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WensExists(id))
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
        /// Wens toevoegen aan wishlist
        /// </summary>
        // POST: api/Wensen
        [HttpPost]
        [Route("~/api/Wishlists/{wishlistId}/Wensen")]
        public async Task<IActionResult> PostWens([FromRoute] int wishlistId, [FromBody] Wens wens)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = int.Parse(User.Claims.SingleOrDefault(t => t.Type == "id")?.Value);
            var gebruiker = _context.Gebruikers
                .Include(t => t.EigenWishlists).ThenInclude(t => t.Wensen)
                .SingleOrDefault(t => t.Id == id);

            var wishlist = _context.Wishlists.SingleOrDefault(t => t.Id == wishlistId);

            gebruiker.WensToevoegenAanWishlist(wishlist, wens);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWens", new { id = wens.Id }, wens);
        }

        /// <summary>
        /// Wens verwijderen
        /// </summary>
        // DELETE: api/Wensen/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWens([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wens = await _context.Wensen.SingleOrDefaultAsync(m => m.Id == id);
            if (wens == null)
            {
                return NotFound();
            }

            _context.Wensen.Remove(wens);
            await _context.SaveChangesAsync();

            return Ok(wens);
        }

        private bool WensExists(int id)
        {
            return _context.Wensen.Any(e => e.Id == id);
        }
    }
}
