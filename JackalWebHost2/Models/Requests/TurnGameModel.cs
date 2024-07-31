namespace JackalWebHost2.Models.Requests
{
    public class TurnGameModel
    {
        public string GameName { get; set; }
        public int? TurnNum { get; set; }
        public Guid? PirateId { get; set; }
    }
}
