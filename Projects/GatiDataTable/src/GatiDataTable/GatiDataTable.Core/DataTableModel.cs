
namespace GatiDataTable.Core
{
    public sealed class DataTableModel
    {
        public DataTableSchema Schema { get; }

        public List<DataRowModel> Rows { get; } = [];

        private long _idSeed = 0;

        public DataTableModel(DataTableSchema schema)
        {
            Schema = schema;
        }

        public DataRowModel AddRow()
        {
            var row = new DataRowModel(Schema);
            row.Set("Id", NextId());
            Rows.Add(row);
            return row;
        }

        private uint NextId()
        {
            return unchecked((uint)Interlocked.Increment(ref _idSeed));
        }

        public void AddColumn(
            string name, 
            ColumnKind kind, 
            string? enumTypeName = null,
            bool isNullable = false, 
            bool isUnsigned = false, 
            object? defaultValue = null)
        {
            Schema.AddColumn(name, kind, enumTypeName, isNullable, isUnsigned, defaultValue);
            foreach (var row in Rows)
            {
                row.AddColumnDefault(defaultValue);
            }
        }
    }
}
