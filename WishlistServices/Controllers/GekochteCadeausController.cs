using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WishlistServices.Data;
using WishlistServices.Models;

namespace WishlistServices.Controllers
{
    [Produces("application/json")]
    [Route("api/GekochteCadeaus")]
    public class GekochteCadeausController : Controller
    {
        private readonly WishlistDbContext _context;

        public GekochteCadeausController(WishlistDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gekocht cadeau toevoegen aan wens
        /// </summary>
        // POST: api/GekochteCadeaus
        [HttpPost]
        [Route("~/api/Wens/{wensId}/GekochteCadeaus")]
        public async Task<IActionResult> PostGekochtCadeau([FromRoute] int wensId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = int.Parse(User.Claims.SingleOrDefault(t => t.Type == "id")?.Value);
            var gebruiker = _context.Gebruikers
                .Include(t => t.Wishlists).ThenInclude(t => t.Wishlist).ThenInclude(t => t.Wensen).ThenInclude(t => t.GekochtCadeau)
                .SingleOrDefault(t => t.Id == id);

            var wens = _context.Wensen.SingleOrDefault(t => t.Id == wensId);
            wens.MarkerenAlsGekocht(gebruiker);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Gekocht cadeau verwijderen van wens
        /// </summary>
        // DELETE: api/GekochteCadeaus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGekochtCadeau([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var gekochtCadeau = await _context.GekochtCadeaus.SingleOrDefaultAsync(m => m.Id == id);
            if (gekochtCadeau == null)
            {
                return NotFound();
            }

            _context.GekochtCadeaus.Remove(gekochtCadeau);
            await _context.SaveChangesAsync();

            return Ok(gekochtCadeau);
        }

        private bool GekochtCadeauExists(int id)
        {
            return _context.GekochtCadeaus.Any(e => e.Id == id);
        }
    }
}