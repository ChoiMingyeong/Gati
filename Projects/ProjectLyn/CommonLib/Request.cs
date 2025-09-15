using MessagePack;

namespace CommonLib
{
    public class Request
    {
        [MessagePackObject]
        public class LoginInfo
        {
            [Key(0)]
            public string? Id { get; set; }
        }
    }
}
