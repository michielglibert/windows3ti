using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Test.Data;

namespace Test.Controllers
{
    [Produces("application/json")]
    [Route("api/Authentication")]
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        private readonly WishlistDbContext _context;

        public AuthenticationController(WishlistDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult RequestToken([FromBody] TokenRequest request)
        {
            var gebruiker = _context.Gebruikers.SingleOrDefault(t => t.Username == request.Username);

            if (gebruiker == null)
            {
                return NotFound();
            }

            if (string.Equals(request.Username, gebruiker.Username, StringComparison.CurrentCultureIgnoreCase)
                && string.Equals(request.Password, gebruiker.Password))
            {

                var claims = new[]
                {
                    new Claim("username", request.Username),
                    new Claim("id", gebruiker.Id.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsVeryUnsafeButThisIsOnlyASchoolProject"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "hogent.be",
                    audience: "hogent.be",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(3000000),
                    signingCredentials: creds);
                

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return BadRequest("Could not verify username and password");
        }

        private bool GebruikerExists(int id)
        {
            return _context.Gebruikers.Any(e => e.Id == id);
        }

    }

    public class TokenRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}