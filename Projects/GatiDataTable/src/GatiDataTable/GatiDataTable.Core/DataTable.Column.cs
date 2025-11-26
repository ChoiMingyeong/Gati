
using TSID.Creator.NET;

namespace GatiDataTable.Core
{
    public class DataTableException : Exception;

    public sealed partial class DataTable
    {
        public sealed class Column : IColumn
        {
            private readonly DataTable _table;

            public ID ID { get; init; }

            public string Name { get; set; } = string.Empty;

            public string? Description { get; set; }

            public Type Type
            {
                get
                {
                    return _type;
                }

                set
                {
                    if (!value.IsValueType
                        || !typeof(IConvertible).IsAssignableFrom(value))
                    {
                        throw new ArgumentException(nameof(value));
                    }

                    if (Rows.All(p => p.CanChangeType(value)))
                    {
                        _type = value;
                    }
                    else
                    {
                        throw new DataTableException();
                    }
                }
            }
            private Type _type;

            public bool IsNullable => Nullable.GetUnderlyingType(Type) is not null;

            public Column(in DataTable dataTable, ID id, string name, Type type)
            {
                _table = dataTable;
                ID = id;
                Name = name;
                Type = type;
            }

            public Column(in DataTable dataTable, string name, Type type)
            {
                _table = dataTable;
                ID = TsidCreator.GetTsid();
                Name = name;
                Type = type;
            }
        }
    }
}
