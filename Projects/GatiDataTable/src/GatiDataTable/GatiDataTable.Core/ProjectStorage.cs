using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;

namespace GatiDataTable.Core
{
    public static class ProjectStorage
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }, // ColumnKind를 문자열로 저장
        };

        public static void SaveProject(
            string filePath,
            string projectName,
            IEnumerable<DataTableEntry> tables)
        {
            var dto = new ProjectDto
            {
                Name = projectName,
                Tables = tables.Select(ProjectMapper.ToDto).ToList()
            };

            var json = JsonSerializer.Serialize(dto, JsonOptions);
            File.WriteAllText(filePath, json, Encoding.UTF8);
        }

        public static ProjectDto LoadProject(string filePath)
        {
            var json = File.ReadAllText(filePath, Encoding.UTF8);
            var dto = JsonSerializer.Deserialize<ProjectDto>(json, JsonOptions)
                      ?? throw new InvalidDataException("프로젝트 파일을 읽을 수 없습니다.");
            return dto;
        }
    }
}
