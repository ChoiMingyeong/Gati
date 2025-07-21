namespace Common.Models
{

    public abstract class IDataTable<TColumn, TRow> 
        where TColumn : IColumn 
        where TRow : IRow
    {
        public required Guid Guid { get; init; }

        public required Version Version { get; init; }

        public required string Name { get; set; }

        public List<TColumn> Columns { get; set; } = [];
        
        public List<TRow> Rows { get; set; } = [];
    }
}
