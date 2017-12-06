using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WishlistServices.Models;

namespace WishlistServices.Controllers
{
    [Produces("application/json")]
    [Route("api/Gebruikers")]
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
            return _context.Gebruikers;
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