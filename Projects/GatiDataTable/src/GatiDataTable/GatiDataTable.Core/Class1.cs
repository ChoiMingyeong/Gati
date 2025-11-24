using System.Runtime.CompilerServices;

namespace GatiDataTable.Core
{
    public readonly record struct DataTableID(uint Value)
    {
        public static implicit operator DataTableID(uint id) => new(id);
        public static implicit operator uint(DataTableID id) => ;
    }

    public interface IDataTable
    {

    }
}
