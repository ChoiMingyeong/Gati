namespace GatiDataTable.Core
{
    public sealed class ProjectDto
    {
        public string Name { get; set; } = string.Empty;

        public List<TableDto> Tables { get; set; } = [];
    }
}
