
namespace GatiDataTable.Core
{
    public sealed class ColumnDefinition
    {
        public string Name { get; set; }

        public ColumnKind Kind { get; set; }

        public string? EnumTypeName { get; set; }

        public bool IsNullable { get; set; }

        public bool IsUnsigned { get; set; }

        public bool IsSystem { get; }

        public bool IsReadOnly { get; }

        public ColumnDefinition(
            string name, 
            ColumnKind columnKind = ColumnKind.Int, 
            string? enumTypeName = null,
            bool isNullable = false, 
            bool isUnsigned = false,
            bool isSystem = false,
            bool isReadOnly = false)
        {
            Name = name;
            Kind = columnKind;
            EnumTypeName = enumTypeName;
            IsNullable = isNullable;
            IsUnsigned = isUnsigned;
            IsSystem = isSystem;
            IsReadOnly = isReadOnly;
        }

        public bool TryChangeType(
            ColumnKind kind,
            string? enumTypeName = null,
            bool isNullable = false,
            bool isUnsigned = false)
        {
            if(IsSystem)
            {
                return false;
            }

            Kind = kind;
            EnumTypeName = enumTypeName;

            return true;
        }
    }
}
