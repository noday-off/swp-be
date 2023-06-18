using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SWP391_PreCookingPackage.Models;
using SWP391_PreCookingPackage.ModelsDTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SWP391_PreCookingPackage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly PrecookContext _context;
        private readonly IMapper _mapper;

        public AuthController(PrecookContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public ActionResult<User> Login(UserLoginModel model)
        {
            var users = _context.Users.ToList();
            if (!users.Any(x => x.Username == model.Username))
            {
                return BadRequest("User not found.");
            }

            if (!users.Any(x => x.Password == model.Password))
            {
                return BadRequest("Wrong password.");
            }

            string token = CreateToken(users.Find(x => x.Username == model.Username && x.Password == model.Password));

            return Ok(token);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, ((Role)user.Role).ToString()),
                new Claim(JwtRegisteredClaimNames.Aud, _configuration["AppSettings:Audience"]),
                new Claim(JwtRegisteredClaimNames.Iss, _configuration["AppSettings:Issuer"])
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new SecurityTokenDescriptor
            {
                Issuer = _configuration["AppSettings:Audience"],
                Audience = _configuration["AppSettings:Issuer"],
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var new_token = new JwtSecurityToken
            (
                issuer: _configuration["Appsettings:Issuer"],
                audience: _configuration["AppSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().CreateToken(token);
            var encrypterJwt = new JwtSecurityTokenHandler().WriteToken(new_token);

            return encrypterJwt;
        }
    }
}
