namespace GatiDataTable.Core
{
    public interface IRow : IDataTableObject
    {
        object? Value { get; }
    }
}
