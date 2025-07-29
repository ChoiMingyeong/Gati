using MemoryPack;

namespace WebCore.Shared;

[MemoryPackable]
public partial class ChatMessage
{
    public string Message { get; set; } = string.Empty;
}