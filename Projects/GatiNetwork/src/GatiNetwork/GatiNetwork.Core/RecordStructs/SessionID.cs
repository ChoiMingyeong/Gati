using TSID.Creator.NET;

namespace GatiNetwork.Core.RecordStructs
{
    public readonly record struct SessionID(Tsid Value)
    {
        public static explicit operator Tsid(SessionID sessionID) => sessionID.Value;
        public static implicit operator SessionID(Tsid value) => new(value);
    }
}
