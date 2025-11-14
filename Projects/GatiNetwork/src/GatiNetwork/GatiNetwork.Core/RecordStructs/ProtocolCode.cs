namespace GatiNetwork.Core.RecordStructs
{
    public readonly record struct ProtocolCode(int Value)
    {
        public static explicit operator int(ProtocolCode protocolCode) => protocolCode.Value;
        public static implicit operator ProtocolCode(int value) => new ProtocolCode(value);
    }
}
