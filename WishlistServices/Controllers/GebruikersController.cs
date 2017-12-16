using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
    [Route("api/Gebruikers")]
    [Authorize]
    public class GebruikersController : Controller
    {
        private readonly WishlistDbContext _context;

        public GebruikersController(WishlistDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get's alle gebruikers en alle attributen (handig voor testen)
        /// </summary>
        [HttpGet]
        public IEnumerable<Gebruiker> GetGebruiker()
        {
            //var id = int.Parse(User.Claims.SingleOrDefault(t => t.Type == "id")?.Value);
            return _context.Gebruikers.Include(t => t.EigenWishlists)
                .Include(t => t.EigenWishlists).ThenInclude(t => t.Ontvanger)
                .Include(t => t.EigenWishlists).ThenInclude(t => t.VerzondenUitnodigingen)
                .Include(t => t.EigenWishlists).ThenInclude(t => t.Requests)
                .Include(t => t.EigenWishlists).ThenInclude(t => t.Wensen).ThenInclude(t => t.GekochtCadeau)
                .Include(t => t.Wishlists).ThenInclude(t => t.Wishlist).ThenInclude(t => t.Ontvanger)
                .Include(t => t.Wishlists).ThenInclude(t => t.Wishlist).ThenInclude(t => t.VerzondenUitnodigingen)
                .Include(t => t.Wishlists).ThenInclude(t => t.Wishlist).ThenInclude(t => t.Requests)
                .Include(t => t.Wishlists).ThenInclude(t => t.Wishlist).ThenInclude(t => t.Wensen).ThenInclude(t => t.GekochtCadeau)
                .Include(t => t.Uitnodigingen).ThenInclude(t => t.Gebruiker)
                .Include(t => t.Uitnodigingen).ThenInclude(t => t.Wishlist)
                .Include(t => t.Requests).ThenInclude(t => t.Gebruiker)
                .Include(t => t.Requests).ThenInclude(t => t.Wishlist);
        }

        /// <summary>
        /// Gebruiker zoeken by naam: .../search?naam=Jef
        /// </summary>
        [HttpGet]
        [Route("~/api/Gebruikers/search")]
        public IEnumerable<Gebruiker> GetGebruikerBySearch([FromQuery] string naam)
        {
            if (naam.Length < 3)
            {
                throw new ArgumentException("Zoekopdracht moet langer zijn dan 3 tekens");
            }
            return _context.Gebruikers.Where(t => t.Username.Contains(naam));
        }

        /// <summary>
        /// Gebruiker opvragen by id
        /// </summary>
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

        /// <summary>
        /// Gebruiker opvragen by username
        /// </summary>
        // GET: api/Gebruikers/ByUsername/Jef
        [HttpGet("ByUsername/{username}")]
        public async Task<IActionResult> GetGebruiker([FromRoute] string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var gebruiker = await _context.Gebruikers.SingleOrDefaultAsync(g => g.Username.Equals(username));

            if (gebruiker == null)
            {
                return NotFound();
            }

            return Ok(gebruiker);
        }

        /// <summary>
        /// Gebruiker aanpassen
        /// </summary>
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
        
        private bool GebruikerExists(int id)
        {
            return _context.Gebruikers.Any(e => e.Id == id);
        }
    }
}