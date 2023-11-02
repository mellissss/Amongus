using Amongus.Api.Auth;
using Amongus.Api.Dto;
using Amongus.Domain;
using Amongus.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Amongus.Api.Controllers
{
    public class AccountController : BaseApiController
    {
        public AccountController(AmongusContext context) : base(context)
        {
        }

        [HttpPost("[action]")]
        public IActionResult Login(LoginDto input)
        {
            var identity = GetIdentity(input.Username, input.Password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    expires: DateTime.Now.AddDays(120),
                    claims: identity.Claims,
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };
            return Ok(response);
        }

        [HttpPost("[action]")]
        public IActionResult Register(CreateUserDto input)
        {
            Context.Users.Add(new User
            {
                Login = input.Login,
                Password = input.Password,
                Nickname = input.Login
            });
            Context.SaveChanges();
            return Ok(input);
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var person = Context.Users.FirstOrDefault(x => x.Login == username && x.Password == password);
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }
    }
}
