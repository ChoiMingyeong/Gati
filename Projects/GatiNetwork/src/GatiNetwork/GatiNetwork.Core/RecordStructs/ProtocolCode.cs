namespace GatiNetwork.Core.RecordStructs
{
    public readonly record struct ProtocolCode(ushort Value)
    {
        public static explicit operator ushort(ProtocolCode protocolCode) => protocolCode.Value;
        public static implicit operator ProtocolCode(ushort value) => new(value);
    }
}
