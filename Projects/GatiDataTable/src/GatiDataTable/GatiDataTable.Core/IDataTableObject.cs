namespace GatiDataTable.Core
{
    public interface IDataTableObject
    {
        ID ID { get; }

        string? Name { get; }

        string? Description { get; }
    }
}
