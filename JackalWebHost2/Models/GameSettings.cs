namespace JackalWebHost.Models
{
    public class GameSettings
    {
        public string[] Players { get; set; } = null!;
        public int? MapId { get; set; }
    }
}