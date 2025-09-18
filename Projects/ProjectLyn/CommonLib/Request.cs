using MessagePack;

namespace CommonLib
{
    public class Request
    {
        [MessagePackObject]
        public class LoginInfo
        {
            [Key(0)]
            public string? ConnectionId { get; set; }
            [Key(1)]
            public string? UserEmail { get; set; }
        }
    }
}
