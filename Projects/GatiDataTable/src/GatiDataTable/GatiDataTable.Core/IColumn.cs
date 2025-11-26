namespace GatiDataTable.Core
{
    public interface IColumn : IDataTableObject
    {
        Type Type { get; }
    }

    public interface ICell
    {
        IRow Row { get; }

        IColumn Column { get; }

        object? Value { get; }
    }

    public sealed class Cell : ICell
    {
        public IRow Row { get; init; }

        public IColumn Column { get; init; }

        public object? Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
        private object? _value;

        public Cell(in IRow row, in IColumn column, object? value = null)
        {
            Row = row;
            Column = column;
            Value = value;
        }
    }
}
