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
    [Route("api/Requests")]
    [Authorize]
    public class RequestController : Controller
    {
        private readonly WishlistDbContext _context;

        public RequestController(WishlistDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Alle request voor wishlist bekijken
        /// </summary>
        // GET: api/Request
        [HttpGet]
        [Route("~/api/Wishlists/{wishlistId}/Requests")]
        public IEnumerable<Request> GetRequestsVanWishlist([FromRoute] int wishlistId)
        {
            var wishlist = _context.Wishlists
                .Include(t => t.Requests).ThenInclude(t => t.Gebruiker)
                .SingleOrDefault(t => t.Id == wishlistId);

            return wishlist.Requests;
        }

        /// <summary>
        /// Request by id
        /// </summary>
        // GET: api/Request/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequest([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var request = await _context.Requests.SingleOrDefaultAsync(m => m.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            return Ok(request);
        }

        /// <summary>
        /// Request aanvaard/afwijzen: { "antwoord":true/false }
        /// </summary>
        [HttpDelete]
        [Route("~/api/Requests/{requestId}")]
        public IActionResult RequestBeantwoorden([FromRoute] int requestId, [FromBody] RequestRequest antwoord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var request = _context.Requests
                .Include(t => t.Gebruiker).ThenInclude(t => t.Requests)
                .Include(t => t.Wishlist).ThenInclude(t => t.Requests)
                .SingleOrDefault(t => t.Id == requestId);
            if (request == null)
            {
                return NotFound();
            }

            if (antwoord.Antwoord)
            {
                request.AccepteerRequest();
            }
            else
            {
                request.WijsRequestAf();
            }

            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Request toevoegen
        /// </summary>
        // POST: api/Requests
        [HttpPost]
        public async Task<IActionResult> PostRequest([FromBody] Request request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = int.Parse(User.Claims.SingleOrDefault(t => t.Type == "id")?.Value);
            var gebruiker = _context.Gebruikers.SingleOrDefault(t => t.Id == id);

            var wishlist = _context.Wishlists.SingleOrDefault(t => t.Id == request.Wishlist.Id);

            gebruiker.RequestVersturenVoorWishlist(wishlist);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }
       
    }
}

public class RequestRequest
{
    public bool Antwoord { set; get; }
}
