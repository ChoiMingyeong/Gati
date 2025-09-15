using MessagePack;

namespace CommonLib
{
    public class Response
    {
        [MessagePackObject]
        public class UserInfo
        {
            [Key(0)]
            public required string Name { get; set; }
            [Key(1)]
            public int Level { get; set; }
        }
    }
}
