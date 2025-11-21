using TSID.Creator.NET;

namespace GatiTest.Shared.RecordStructs
{
    public readonly record struct UserID(Tsid Value)
    {
        public static explicit operator Tsid(UserID userID) => userID.Value;
        public static implicit operator UserID(Tsid value) => new(value);
    }
}
