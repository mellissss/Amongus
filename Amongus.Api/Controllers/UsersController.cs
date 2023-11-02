using Amongus.Api.Dto;
using Amongus.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amongus.Api.Controllers
{
    public class UsersController : BaseApiController
    {
        public UsersController(AmongusContext context) : base(context)
        {
        }
        [HttpGet("[action]")]
        public IActionResult SetNickname(int userId, string nickname) 
        {
            var user = Context.Users.FirstOrDefault(x=>x.Id == userId);

            if (user == null)
            {
                return BadRequest("Такого пользователя не существует.");     
            }

            user.Nickname = nickname;
            Context.Users.Update(user);
            Context.SaveChanges();
            return Ok();
        }

        [HttpGet("[action]")]
        public IActionResult GetInfo()
        {
            var login = User.Identity.Name;

            var user = Context.Users.Include(x=> x.Room).FirstOrDefault(x => x.Login == login);
            if (user != null)
            {
                var result = new GetUserInfoDto
                {
                    CurrentRoomId = user.RoomId,
                    IsStartGame = user.Room?.IsStartGame ?? false,
                };
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
