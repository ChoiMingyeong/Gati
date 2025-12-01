namespace GatiDataTable.Core
{
    public static class ProjectMapper
    {
        public static ColumnDto ToDto(ColumnDefinition col)
        {
            return new ColumnDto
            {
                Name = col.Name,
                Kind = col.Kind,
                EnumTypeName = col.EnumTypeName,
                IsNullable = col.IsNullable,
                IsUnsigned = col.IsUnsigned,
                DefaultValue = col.DefaultValue,
                IsSystem = col.IsSystem,
                IsReadOnly = col.IsReadOnly,
            };
        }

        public static ColumnDefinition FromDto(ColumnDto dto)
        {
            return new ColumnDefinition(
                dto.Name,
                dto.Kind,
                enumTypeName: dto.EnumTypeName,
                isNullable: dto.IsNullable,
                isUnsigned: dto.IsUnsigned,
                defaultValue: dto.DefaultValue,
                isSystem: dto.IsSystem,
                isReadOnly: dto.IsReadOnly);
        }

        public static TableDto ToDto(DataTableEntry entry)
        {
            var model = entry.Table;
            var schema = model.Schema;

            var tableDto = new TableDto
            {
                Name = schema.Name,
                Columns = [.. schema.Columns.Select(ToDto)],
            };

            foreach (var row in model.Rows)
            {
                var rowDto = new RowDto();
                foreach (var col in schema.Columns)
                {
                    var value = row[col.Name];
                    rowDto.Values.Add(SerializeValue(col.Kind, value));
                }
                tableDto.Rows.Add(rowDto);
            }

            return tableDto;
        }

        public static DataTableEntry FromDto(TableDto dto)
        {
            var schema = new DataTableSchema(dto.Name);
            
            schema.ClearColumnsForLoad();
            foreach (var colDto in dto.Columns)
            {
                schema.AddColumnFromDefinition(FromDto(colDto));
            }

            var model = new DataTableModel(schema);

            foreach (var rowDto in dto.Rows)
            {
                var row = new DataRowModel(schema);
                for (int i = 0; i < schema.Columns.Count; i++)
                {
                    var col = schema.Columns[i];
                    var raw = i < rowDto.Values.Count ? rowDto.Values[i] : null;
                    var value = DeserializeValue(col.Kind, raw);
                    row[col.Name] = value;
                }

                model.Rows.Add(row);
            }

            model.ReseedIdFromExistingRows();

            return new DataTableEntry(model);
        }

        private static string? SerializeValue(ColumnKind kind, object? value)
        {
            if (value is null)
                return null;

            return kind switch
            {
                ColumnKind.Int => Convert.ToInt64(value).ToString(),
                ColumnKind.Float => Convert.ToDouble(value).ToString(System.Globalization.CultureInfo.InvariantCulture),
                ColumnKind.Bool => ((bool)value).ToString(),
                //ColumnKind.Enum => value.ToString(), // 이름 또는 숫자 문자열 그대로 저장
                ColumnKind.String => (string)value,
                _ => value.ToString()
            };
        }

        private static object? DeserializeValue(ColumnKind kind, string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return kind switch
                {
                    ColumnKind.Int => 0,
                    ColumnKind.Float => 0f,
                    ColumnKind.Bool => false,
                    //ColumnKind.Enum => null,
                    ColumnKind.String => string.Empty,
                    _ => null
                };
            }

            return kind switch
            {
                ColumnKind.Int => long.TryParse(text, out var i) ? i : 0,
                ColumnKind.Float => double.TryParse(text, System.Globalization.NumberStyles.Any,
                                                    System.Globalization.CultureInfo.InvariantCulture, out var d)
                                    ? d : 0d,
                ColumnKind.Bool => bool.TryParse(text, out var b) ? b : false,
                //ColumnKind.Enum => text, // 나중에 실제 Enum 타입으로 변환
                ColumnKind.String => text,
                _ => text
            };
        }
    }
}
