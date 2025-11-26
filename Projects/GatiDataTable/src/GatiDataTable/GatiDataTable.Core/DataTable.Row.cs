namespace GatiDataTable.Core
{
    public sealed partial class DataTable
    {
        public sealed class Row(in Column column, ID id) : IRow
        {
            private readonly Column _column = column;

            public ID ID { get; init; } = id;

            public required string Name { get; set; }

            public string? Description { get; set; }

            public object? Value
            {
                get
                {
                    return Convert.ChangeType(_value, _column.Type);
                }
                set
                {
                    if (!CanChangeType(_column.Type))
                    {
                        throw new DataTableException();
                    }

                    _value = value;
                }
            }
            private object? _value;

            public bool CanChangeType(Type targetType)
            {
                if (_value is null)
                {
                    return _column.IsNullable;
                }

                var valueType = _value.GetType();
                if (targetType.IsAssignableFrom(valueType))
                {
                    return true;
                }

                if (_value is IConvertible)
                {
                    return true;
                }

                var underlying = Nullable.GetUnderlyingType(targetType);
                if (underlying != null)
                {
                    return CanChangeType(underlying);
                }

                return false;
            }
        }
    }
}