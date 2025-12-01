namespace GatiDataTable.Core
{
    public sealed class DataTableEntry
    {
        public string Name => Table.Schema.Name;

        public DataTableModel Table { get; }

        public DataTableEntry(DataTableModel table)
        {
            Table = table;
        }

        public override string ToString() => Name;
    }
}