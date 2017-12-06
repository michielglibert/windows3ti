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
    [Route("api/Wensen")]
    public class WensController : Controller
    {
        private readonly WishlistDbContext _context;

        public WensController(WishlistDbContext context)
        {
            _context = context;
        }

        // GET: api/Wens
        [HttpGet]
        public IEnumerable<Wens> GetWensen()
        {
            return _context.Wensen;
        }

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

        // POST: api/Wensen
        [HttpPost]
        public async Task<IActionResult> PostWens([FromBody] Wens wens)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Wensen.Add(wens);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWens", new { id = wens.Id }, wens);
        }

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
