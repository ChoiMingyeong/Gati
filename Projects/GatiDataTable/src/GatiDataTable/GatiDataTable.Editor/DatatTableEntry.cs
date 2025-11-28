using GatiDataTable.Core;

namespace GatiDataTable.Editor
{
    public sealed class DatatTableEntry
    {
        public string Name => Table.Schema.Name;

        public DataTableModel Table { get; }

        public DatatTableEntry(DataTableModel table)
        {
            Table = table;
        }

        public override string ToString() => Name;
    }
}