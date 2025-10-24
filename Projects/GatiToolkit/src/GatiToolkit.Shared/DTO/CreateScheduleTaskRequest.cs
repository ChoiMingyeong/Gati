namespace GatiToolkit.Shared.DTO
{
    public class CreateScheduleTaskRequest
    {
        public enum RepeatTypes
        {
            None,
            Daily,
            Weekly,
        }

        public string CalendarID { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public RepeatTypes RepeatType { get; set; } = RepeatTypes.None;

        public int RepeatCount { get; set; } = 1;
    }
}
