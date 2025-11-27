
namespace GatiDataTable.Core
{
    public sealed class ColumnDefinition
    {
        public string Name { get; set; }

        public ColumnKind Kind { get; set; }

        public string? EnumTypeName { get; set; }

        public bool IsNullable { get; set; }

        public bool IsUnsigned { get; set; }

        public ColumnDefinition(
            string name, 
            ColumnKind columnKind = ColumnKind.Int, 
            string? enumTypeName = null,
            bool isNullable = false, 
            bool isUnsigned = false)
        {
            Name = name;
            Kind = columnKind;
            EnumTypeName = enumTypeName;
            IsNullable = isNullable;
            IsUnsigned = isUnsigned;
        }
    }
}
