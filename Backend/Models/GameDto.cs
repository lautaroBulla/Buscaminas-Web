namespace Backend.Models
{
    public class GameDto
    {
        public required int Help { get; set; }
        public required decimal Seconds { get; set; }
        public required string Difficulty { get; set; }
        public required int Rows { get; set; }
        public required int Cols { get; set; }
        public required int Mines { get; set; }
        public required int N3BV { get; set; }
        public required int Clicks { get; set; }
        public required int Efficiency { get; set; }
    }
}
