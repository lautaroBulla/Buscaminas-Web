namespace Backend.Models
{
    public class ResponseDifficulty
    {
        public required int rows { get; set; }
        public required int cols { get; set; }
        public required int mines { get; set; }
        public required int totalPages { get; set; }
        public required int page { get; set; }
        public required MyPositionDto? myPosition { get; set; }
        public required List<GamesInfoDto> games { get; set; }
    }

    public class GamesInfoDto
    {
        public required int id { get; set; }
        public required int help { get; set; }
        public required decimal seconds { get; set; }
        public required int n3BV { get; set; }
        public required int clicks { get; set; }
        public required int efficiency { get; set; }
        public required DateTime createdAt { get; set; }
        public required UserInfoDto user { get; set; }
    }
    
    public class UserInfoDto
    {
        public required string username { get; set; }
    }
}
