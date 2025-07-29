using MemoryPack;

namespace WebCore.Shared;

[MemoryPackable]
public partial class Packet
{
    public ushort Opcode { get; set; }
    public byte[] Payload { get; set; } = [];
}

[MemoryPackable]
public partial class ChatMessage
{
    public string Message { get; set; } = string.Empty;
}