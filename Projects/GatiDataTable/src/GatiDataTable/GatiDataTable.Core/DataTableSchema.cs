
namespace GatiDataTable.Core
{
    public sealed class DataTableSchema
    {
        public IReadOnlyList<ColumnDefinition> Columns => _columns;
        private readonly List<ColumnDefinition> _columns = [];

        public string Name { get; set; }

        public DataTableSchema(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException($"DataSchema Name");
            }

            Name = name;
        }

        public DataTableSchema AddColumn(string name, ColumnKind kind, string? enumTypeName = null, bool isNullable = false, bool isUnsigned = false)
        {
            _columns.Add(new ColumnDefinition(name, kind, enumTypeName, isNullable, isUnsigned));
            return this;
        }

        public int GetColumnIndex(string name)
        {
            for (int i = 0; i < _columns.Count; i++)
            {
                if (string.Equals(_columns[i].Name, name, StringComparison.Ordinal))
                {
                    return i;
                }
            }

            throw new KeyNotFoundException($"Column '{name}' not found.");
        }

        public void SetName(string name)
        {
            if (string.Equals(Name, name, StringComparison.Ordinal))
            {
                return;
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException($"DataSchema Need Name");
            }

            Name = name;
        }
    }
}
