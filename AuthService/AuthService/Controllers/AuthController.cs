using AuthService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly ApplicationContext _context;
        public AuthController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("Log")]
        public async Task<IActionResult> Login(string name, string password)
        {
            List<User> Users = await _context.Users.ToListAsync();
            User? person = Users.FirstOrDefault(p => p.Name == name && p.Password == password);
            if (person is null) return (IActionResult)Results.Unauthorized();

            string encodedJwt = GenerateToken(person);

            var response = new
            {
                access_token = encodedJwt,
                username = person.Name
            };

            return Json(response);
        }
        [HttpGet("Reg")]
        public async Task<IActionResult> Registration(string name, string password) {
            var user = _context.Users.Where(x => x.Name == name);
            if (user.Count()>0)
                return BadRequest("пользователь уже есть");
            
            User? person = new()
            {
                Name = name,
                Password = password
            };

            _context.Users.Add(person);
            await _context.SaveChangesAsync();

            string encodedJwt = GenerateToken(person);
            
            var response = new
            {
                access_token = encodedJwt,
                username = person.Name
            };
            return Json(response);
        }
        string GenerateToken(User person) {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Name) };
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }


    }
}
