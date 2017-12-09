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
using WishlistServices.Data;
using WishlistServices.Models;

namespace WishlistServices.Controllers
{
    [Produces("application/json")]
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        private readonly WishlistDbContext _context;

        public AuthenticationController(WishlistDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gebruiker registreren, geeft json token terug
        /// </summary>
        [HttpPost]
        [Route("api/Authentication/register")]
        public IActionResult Register([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Gebruiker gebruiker = new Gebruiker(user.Username, user.Password);

            _context.Gebruikers.Add(gebruiker);

            _context.SaveChanges();

            return Login(user);
        }

        /// <summary>
        /// Gebruiker inloggen, geeft json token terug.
        /// </summary>
        [HttpPost]
        [Route("api/Authentication/login")]
        public IActionResult Login([FromBody] User user)
        {
            var gebruiker = _context.Gebruikers.SingleOrDefault(t => t.Username == user.Username);

            if (gebruiker == null)
            {
                return NotFound();
            }

            if (string.Equals(user.Username, gebruiker.Username, StringComparison.CurrentCultureIgnoreCase)
                && string.Equals(user.Password, gebruiker.Password))
            {

                var claims = new[]
                {
                    new Claim("username", user.Username),
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

    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}