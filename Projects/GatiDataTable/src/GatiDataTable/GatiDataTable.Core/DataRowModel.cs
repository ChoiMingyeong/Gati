
namespace GatiDataTable.Core
{
    public sealed class DataRowModel
    {
        private readonly DataTableSchema _schema;
        private readonly List<object?> _values;

        public DataRowModel(DataTableSchema schema)
        {
            _schema = schema;
            _values = new List<object?>(_schema.Columns.Count);
            foreach (var _ in _schema.Columns)
            {
                _values.Add(null);
            }
        }

        public object? this[int index]
        {
            get => _values[index];
            set => _values[index] = value;
        }

        public object? this[string columnName]
        {
            get
            {
                return _values[_schema.GetColumnIndex(columnName)];
            }
            set
            {
                _values[_schema.GetColumnIndex(columnName)] = value;
            }
        }

        public T? Get<T>(string columnName) => (T?)this[columnName];

        public void Set<T>(string columnName, T value) => this[columnName] = value;

        public void AddColumnDefault(object? defaultValue)
        {
            _values.Add(defaultValue);
        }
    }
}
