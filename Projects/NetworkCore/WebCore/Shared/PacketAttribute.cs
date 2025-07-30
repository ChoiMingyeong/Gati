namespace WebCore.Shared;

[AttributeUsage(AttributeTargets.Class)]
public class PacketAttribute : Attribute
{
    public ushort Opcode { get; init; }

    public bool IsCompressed { get; set; }

    public PacketAttribute(ushort opcode, bool isCompressed = false)
    {
        Opcode = opcode;
        IsCompressed = isCompressed;
    }
}