namespace Amongus.Api.Dto
{
    public class CreateRoomDto
    {
        public string Name { get; set; }
        public string? Password { get; set; }
        public int CreatorId { get; set; }
    }
}
