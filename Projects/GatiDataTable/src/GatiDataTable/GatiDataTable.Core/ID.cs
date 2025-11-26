using TSID.Creator.NET;

namespace GatiDataTable.Core
{
    public readonly record struct ID
    {
        private readonly Tsid _value;

        public ID(Tsid value)
        {
            _value = value;
        }

        public ID(long value)
        {
            _value = Tsid.From(value);
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public static implicit operator long(ID id) => id._value.ToLong();
        public static implicit operator Tsid(ID id) => id._value;
        public static implicit operator ID(long value) => new (Tsid.From(value));
        public static implicit operator ID(Tsid value) => new (value);
    }
}
