
namespace GatiDataTable.Core
{
    public sealed class DataTableModel
    {
        public DataTableSchema Schema { get; }

        public List<DataRowModel> Rows { get; } = [];

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
    }
}
