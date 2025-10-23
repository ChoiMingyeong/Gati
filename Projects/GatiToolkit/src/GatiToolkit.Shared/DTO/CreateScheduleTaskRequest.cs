namespace GatiToolkit.Shared.DTO
{
    public class CreateScheduleTaskRequest
    {
        public string CalendarID { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
