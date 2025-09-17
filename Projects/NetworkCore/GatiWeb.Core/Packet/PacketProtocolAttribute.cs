namespace GatiWeb.Core.Packet;

[AttributeUsage(AttributeTargets.Class)]
public class PacketProtocolAttribute : Attribute
{
    public ushort Opcode { get; init; }

    public bool IsCompressed { get; set; }

    public PacketProtocolAttribute(ushort opcode, bool isCompressed = false)
    {
        Opcode = opcode;
        IsCompressed = isCompressed;
    }
}