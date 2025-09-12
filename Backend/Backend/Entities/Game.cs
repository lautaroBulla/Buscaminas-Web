using Microsoft.EntityFrameworkCore;

namespace Backend.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public required int Help { get; set; }
        [Precision(8, 3)]
        public required decimal Seconds { get; set; }
        public required string Difficulty { get; set; }
        public required int Rows { get; set; }
        public required int Cols { get; set; }
        public required int Mines { get; set; }
        public required int N3BV { get; set; }
        public required int Clicks { get; set; }
        public required int Efficiency { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
