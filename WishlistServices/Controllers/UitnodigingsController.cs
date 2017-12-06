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
    [Route("api/Uitnodigingen")]
    public class UitnodigingsController : Controller
    {
        private readonly WishlistDbContext _context;

        public UitnodigingsController(WishlistDbContext context)
        {
            _context = context;
        }

        // GET: api/Uitnodigingen
        [HttpGet]
        public IEnumerable<Uitnodiging> GetUitnodigingen()
        {
            return _context.Uitnodigingen;
        }

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

        // PUT: api/Uitnodigingen/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUitnodiging([FromRoute] int id, [FromBody] Uitnodiging uitnodiging)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != uitnodiging.Id)
            {
                return BadRequest();
            }

            _context.Entry(uitnodiging).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UitnodigingExists(id))
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

        // POST: api/Uitnodigingen
        [HttpPost]
        public async Task<IActionResult> PostUitnodiging([FromBody] Uitnodiging uitnodiging)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Uitnodigingen.Add(uitnodiging);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUitnodiging", new { id = uitnodiging.Id }, uitnodiging);
        }

        // DELETE: api/Uitnodigingen/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUitnodiging([FromRoute] int id)
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

            _context.Uitnodigingen.Remove(uitnodiging);
            await _context.SaveChangesAsync();

            return Ok(uitnodiging);
        }

        private bool UitnodigingExists(int id)
        {
            return _context.Uitnodigingen.Any(e => e.Id == id);
        }
    }
}
