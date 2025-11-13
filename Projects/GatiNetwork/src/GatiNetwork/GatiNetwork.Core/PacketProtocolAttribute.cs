namespace GatiNetwork.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PacketProtocolAttribute(ushort protocolCode) : Attribute
    {
        public ushort ProtocolCode { get; } = protocolCode;
    }
}
