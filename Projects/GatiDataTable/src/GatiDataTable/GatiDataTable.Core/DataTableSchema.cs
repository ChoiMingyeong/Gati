
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

            _columns.Add(new ColumnDefinition(
                "Id",
                ColumnKind.Int, 
                isNullable: false, 
                isUnsigned: true, 
                isSystem: true, 
                isReadOnly: true
                ));
        }

        public DataTableSchema AddColumn(
            string name, 
            ColumnKind kind, 
            string? enumTypeName = null, 
            bool isNullable = false, 
            bool isUnsigned = false,
            object? defaultValue = null)
        {
            if(_columns.Exists(c => string.Equals(c.Name, name, StringComparison.Ordinal)))
            {
                throw new ArgumentException($"Column '{name}' already exists.");
            }

            _columns.Add(new ColumnDefinition(name, kind, 
                enumTypeName: enumTypeName, 
                isNullable: isNullable, 
                isUnsigned: isUnsigned, 
                defaultValue: defaultValue));
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

        public void RemoveAt(int index)
        {
            if (_columns[index].IsSystem)
            {
                throw new InvalidOperationException($"Cannot remove system column '{_columns[index].Name}'.");
            }
            _columns.RemoveAt(index);
        }

        public void Move(int fromIndex, int toIndex)
        {
            if (_columns[fromIndex].IsSystem || _columns[toIndex].IsSystem)
            {
                throw new InvalidOperationException($"Cannot move system column.");
            }

            (_columns[fromIndex], _columns[toIndex]) = (_columns[toIndex], _columns[fromIndex]);
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

        public void ClearColumnsForLoad()
        {
            throw new NotImplementedException();
        }

        internal void AddColumnFromDefinition(ColumnDefinition columnDefinition)
        {
            throw new NotImplementedException();
        }
    }
}
