namespace GatiDataTable.Core
{
    public sealed class ColumnDto
    {
        public string Name { get; set; } = string.Empty;

        public ColumnKind Kind { get; set; }

        public string? EnumTypeName { get; set; }

        public bool IsSystem { get; set; }

        public bool IsReadOnly { get; set; }

        public bool IsNullable { get; set; }

        public bool IsUnsigned { get; set; }

        public object? DefaultValue { get; set; }
    }
}
