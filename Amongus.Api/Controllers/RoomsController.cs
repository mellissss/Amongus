using Amongus.Api.Dto;
using Amongus.Domain;
using Amongus.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amongus.Api.Controllers
{
    public class RoomsController : BaseApiController
    {
        public RoomsController(AmongusContext context) : base(context)
        {
        }

        [HttpPost("[action]")]
        public IActionResult Create(CreateRoomDto input)
        {
            Context.Rooms.Add(new Room
            {
                Name = input.Name,
                Password = input.Password,
                CreatorId = input.CreatorId,
            });
            Context.SaveChanges();
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Exit()
        {
            var login = User.Identity.Name;
            var user = Context.Users.FirstOrDefault(x => x.Login == login);
            if (user != null)
            {
                user.RoomId = null;
                Context.Users.Update(user);
                Context.SaveChanges();
            }
            return Ok();
        }

        [HttpGet("[action]")]
        public IActionResult GetAll()
        {
            var rooms = Context.Rooms
                .Select(x => new RoomDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsPassword = x.Password != null,
                })
                .ToList();
            foreach (var room in rooms)
            {
                room.PlayersCount = Context.Users.Count(x => x.RoomId == room.Id);
            }
            return Ok(rooms);
        }

        [HttpPost("[action]")]
        public IActionResult Enter(EnterRoomDto input)
        {
            var login = User.Identity.Name;
            var user = Context.Users.FirstOrDefault(x => x.Login == login);
            if (user != null)
            {
                user.RoomId = input.RoomId;
                Context.Users.Update(user);
                Context.SaveChanges();
            }
            return Ok();
        }

        [HttpGet("[action]")]
        public IActionResult GetInfo()
        {
            var login = User.Identity.Name;
            var user = Context.Users.FirstOrDefault(x => x.Login == login);
            if (user != null)
            {
                var users = Context.Users.Where(x => x.RoomId == user.RoomId);
                return Ok(new GetRoomInfoDto
                {
                    Users = users.Select(x => new UserReadyDto
                    {
                        IsReady = x.IsReady,
                        Name = x.Nickname
                    })
                }); 
            }
            return Ok();
        }

        [HttpGet("[action]")]
        public IActionResult GetUserIds(int roomId)
        {
            var result = Context.Users.Where(x => x.RoomId == roomId).Select(x=> x.Login).ToArray();
            return Ok(result);
        }
    }
}
