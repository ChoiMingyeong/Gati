namespace GatiNetwork.Core.RecordStructs
{
    public readonly record struct ErrorCode(ushort Value)
    {
        public static implicit operator ushort(ErrorCode errorCode) => errorCode.Value;
        public static implicit operator ErrorCode(ushort value) => new(value);
    }
}
