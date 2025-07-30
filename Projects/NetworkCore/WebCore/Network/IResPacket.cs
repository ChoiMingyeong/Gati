using MemoryPack;
using WebCore.Shared;

namespace WebCore
{
    [MemoryPackable]
    public partial class IResPacket: IPacket
    {
        public ResponseCode ResponseCode { get; set; }
    }
}
