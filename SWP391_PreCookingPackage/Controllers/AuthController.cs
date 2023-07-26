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

        [HttpPost("register")]
        public async Task<ActionResult<UserModel>> Register(UserRegisterModel model)
        {
            try
            {
                var users = _context.Users;
                if(users.Any(x => x.Username == model.Username))
                {
                    return BadRequest("Username has already been registered.");
                }
                model.Role = 1;
                User new_user = _mapper.Map<UserRegisterModel, User>(model);
                //new_user.Role = 1;
                users.Add(new_user);
                await _context.SaveChangesAsync();
                User result = users.OrderByDescending(x => x.Id).FirstOrDefault();
                OrderCreateModel default_cart_order = new OrderCreateModel
                {
                    UserId = result.Id,
                    OrderDate = DateTime.Now,
                    TotalPrice = 0,
                    Status = "On-cart",
                    PaymentMethod = "COD",
                };
                Order default_order = _mapper.Map<Order>(default_cart_order);
                _context.Orders.Add(default_order);
                await _context.SaveChangesAsync();
                //return CreatedAtRoute("GetUserById", new { controller = "users", id = result.Id }, result);
                return Ok(_mapper.Map<UserModel>(result));
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
                return BadRequest("Fail to register");
            }
        }

        [HttpPost("login")]
        public ActionResult Login(UserLoginModel model)
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
            var data = new { token = token };
            return Ok(data);
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
