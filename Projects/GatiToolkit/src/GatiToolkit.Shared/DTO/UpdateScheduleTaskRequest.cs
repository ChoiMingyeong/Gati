namespace GatiToolkit.Shared.DTO
{
    public class UpdateScheduleTaskRequest
    {
        public string CalendarID { get; set; } = string.Empty;

        public string TaskID { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
