namespace Amongus.Api.Dto
{
    public class GetRoomInfoDto
    {
        public IEnumerable<UserReadyDto> Users { get; set; }
    }

    public class UserReadyDto
    {
        public string Name { get; set; }
        public bool IsReady { get; set; }
    }

}
