using Amongus.Api.Dto;
using Amongus.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Security.Principal;

namespace Amongus.Api.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        protected AmongusContext Db { get; set; }

        private static Dictionary<int, List<string>> connections = new Dictionary<int, List<string>>(); //сигналы кто бегает в комнате(все будут видеть информацию, кто играет)
        
        private static Dictionary<string, int> userConnections = new Dictionary<string, int>(); //сигналы кто бегает в комнате(все будут видеть информацию, кто играет)

        public GameHub(AmongusContext db)
        {
            Db = db;
        }

        public void Connect(int roomId) 
        {
            if (!connections.ContainsKey(roomId))
            {
                connections[roomId] = new List<string>();
            }

            if (!userConnections.ContainsKey(Context.ConnectionId))
            {
                userConnections[Context.ConnectionId] = roomId;
            }

            connections[roomId].Add(Context.ConnectionId);
            //Clients.Clients(connections[roomId].Where(x => x != Context.ConnectionId)).SendAsync("Connect", IdentityName).Wait();
        }
        private string IdentityName
        {
            get { return Context.User.Identity.Name; }
        }
        public void RefreshRoom(int roomId)
        {
            Clients.Clients(connections[roomId].Where(x => x != Context.ConnectionId)).SendAsync("RefreshRoom").Wait();
        }

        public void StartGame(int roomId)
        {
            Clients.Clients(connections[roomId].Where(x => x != Context.ConnectionId)).SendAsync("StartGame").Wait();
        }

        public void MoveEnd(string playerLogin)
        {
            var roomId = Db.Users.First(x=>x.Login == playerLogin).RoomId.Value;
            Clients.Clients(connections[roomId].Where(x => x != Context.ConnectionId)).SendAsync("moveEnd", playerLogin).Wait();
        }

        public void Move(MoveDto input)
        {
            var roomId = Db.Users.First(x => x.Login == input.Login).RoomId.Value;
            Clients.Clients(connections[roomId].Where(x => x != Context.ConnectionId)).SendAsync("move", input).Wait();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var roomId = userConnections[Context.ConnectionId];
                connections[roomId].Remove(Context.ConnectionId);
                userConnections.Remove(Context.ConnectionId);
                Clients.Clients(connections[roomId]).SendAsync("Disconnect", IdentityName).Wait();
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("onError", "OnDisconnected: " + ex.Message);
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
