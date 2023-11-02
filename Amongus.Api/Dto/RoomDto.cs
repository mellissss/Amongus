namespace Amongus.Api.Dto
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }   
        public bool IsPassword { get; set; }
        public int PlayersCount { get; set; }
    }
}
