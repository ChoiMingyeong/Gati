namespace ServerLib.Model
{
    public class auth
    {
        public int Id { get; set; }
        public string? ConnectionId { get; set; }
        public int UserId { get; set; }
        public string? UserEmail { get; set; }
    }
}