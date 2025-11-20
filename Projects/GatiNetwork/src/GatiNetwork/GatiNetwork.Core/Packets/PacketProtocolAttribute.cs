using GatiNetwork.Core.RecordStructs;
using System.Reflection;
namespace GatiNetwork.Core.Packets
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PacketProtocolAttribute(ushort protocolCode) : Attribute
    {
        public ProtocolCode ProtocolCode { get; } = protocolCode;
    }
}
