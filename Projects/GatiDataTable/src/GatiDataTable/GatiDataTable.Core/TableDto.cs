namespace GatiDataTable.Core
{
    public sealed class TableDto
    {
        public string Name { get; set; } = string.Empty;

        public List<ColumnDto> Columns { get; set; } = [];

        public List<RowDto> Rows { get; set; } = [];
    }
}
