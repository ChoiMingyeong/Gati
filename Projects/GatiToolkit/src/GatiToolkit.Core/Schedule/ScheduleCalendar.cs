using System.Diagnostics.CodeAnalysis;

namespace GatiToolkit.Core.Schedule
{
    public class ScheduleCalendar : ItemBase
    {
        public readonly List<ScheduleTask> Tasks = [];

        public ScheduleTask? GetTask(string taskID) => Tasks.SingleOrDefault(t => t.ID == taskID);

        public List<ScheduleTask> GetTasks(DateTime dateTime1, DateTime dateTime2)
        {
            if (dateTime1 == dateTime2)
            {
                return [];
            }

            DateTime startDate;
            DateTime endDate;
            (startDate, endDate) = (dateTime1 < dateTime2) ? (dateTime1, dateTime2) : (dateTime2, dateTime1);

            return [.. Tasks.Where(task => task.StartDate >= dateTime1 && task.EndDate <= dateTime2)];
        }

        public string AddTask(DateTime dateTime1, DateTime dateTime2)
        {
            var task = new ScheduleTask(dateTime1, dateTime2);
            Tasks.Add(task);
            return task.ID;
        }

        public bool RemoveTask(string taskID)
        {
            var task = GetTask(taskID);
            if (task == null)
            {
                return false;
            }
            Tasks.Remove(task);
            return true;
        }
    }
}