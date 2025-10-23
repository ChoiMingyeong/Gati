namespace GatiToolkit.Core.Schedule
{
    public class ScheduleTask : ItemBase
    {
        private List<DateTime> _dateTimes = [];

        public DateTime StartDate => _dateTimes.Min();

        public DateTime EndDate => _dateTimes.Max();

        public ScheduleTask(DateTime dateTime1, DateTime dateTime2)
        {
            if (dateTime1 == dateTime2)
            {
                throw new ArgumentException("dateTime1 and dateTime2 cannot be the same.");
            }

            _dateTimes.Clear();
            _dateTimes.Add(dateTime1);
            _dateTimes.Add(dateTime2);
        }

        public void ChangeDateTime(DateTime dateTime1, DateTime dateTime2)
        {
            _dateTimes.Clear();
            _dateTimes.Add(dateTime1);
            _dateTimes.Add(dateTime2);
        }
    }
}