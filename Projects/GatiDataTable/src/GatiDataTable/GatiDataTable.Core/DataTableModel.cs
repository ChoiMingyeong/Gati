
namespace GatiDataTable.Core
{
    public sealed class DataTableModel
    {
        public DataTableSchema Schema { get; }

        public List<DataRowModel> Rows { get; } = [];

        private uint _id = 0;
        public uint Id => Interlocked.Increment(ref _id);

        public DataTableModel(DataTableSchema schema)
        {
            Schema = schema;
        }

        public DataRowModel AddRow()
        {
            var row = new DataRowModel(Schema);
            Rows.Add(row);
            return row;
        }

        public void AddColumn(
            string name, 
            ColumnKind kind, 
            string? enumTypeName = null,
            bool isNullable = false, 
            bool isUnsigned = false, 
            object? defaultValue = null)
        {
            Schema.AddColumn(name, kind, enumTypeName, isNullable, isUnsigned);
            foreach (var row in Rows)
            {
                row.AddColumnDefault(defaultValue);
            }
        }
    }
}
