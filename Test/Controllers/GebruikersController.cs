using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.Data;
using Test.Models;

namespace Test.Controllers
{
    [Produces("application/json")]
    [Route("api/Gebruikers")]
    [Authorize]
    public class GebruikersController : Controller
    {
        private readonly WishlistDbContext _context;

        public GebruikersController(WishlistDbContext context)
        {
            _context = context;
        }

        // GET: api/Gebruikers
        [HttpGet]
        public IEnumerable<Gebruiker> GetGebruiker()
        {
            //var id = User.Claims.SingleOrDefault(t => t.Type == "id")?.Value;
            return _context.Gebruikers.Include(t => t.EigenWishlists);
//                .Include(t => t.Wishlists).ThenInclude(t => t.Wishlist).ThenInclude(t => t.Ontvanger)
//                .Include(t => t.Uitnodigingen).ThenInclude(t => t.Gebruiker)
//                .Include(t => t.Uitnodigingen).ThenInclude(t => t.Wishlist)
//                .Include(t => t.Requests).ThenInclude(t => t.Gebruiker)
//                .Include(t => t.Requests).ThenInclude(t => t.Wishlist);


        }

        // GET: api/Gebruikers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGebruiker([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var gebruiker = await _context.Gebruikers.SingleOrDefaultAsync(m => m.Id == id);

            if (gebruiker == null)
            {
                return NotFound();
            }

            return Ok(gebruiker);
        }

        // PUT: api/Gebruikers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGebruiker([FromRoute] int id, [FromBody] Gebruiker gebruiker)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != gebruiker.Id)
            {
                return BadRequest();
            }

            _context.Entry(gebruiker).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GebruikerExists(id))
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

        // POST: api/Gebruikers
        [HttpPost]
        public async Task<IActionResult> PostGebruiker([FromBody] Gebruiker gebruiker)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Gebruikers.Add(gebruiker);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGebruiker", new { id = gebruiker.Id }, gebruiker);
        }

        // DELETE: api/Gebruikers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGebruiker([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var gebruiker = await _context.Gebruikers.SingleOrDefaultAsync(m => m.Id == id);
            if (gebruiker == null)
            {
                return NotFound();
            }

            _context.Gebruikers.Remove(gebruiker);
            await _context.SaveChangesAsync();

            return Ok(gebruiker);
        }

        private bool GebruikerExists(int id)
        {
            return _context.Gebruikers.Any(e => e.Id == id);
        }
    }
}