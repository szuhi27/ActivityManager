using System.Text;
using System.Xml.Linq;

namespace ActivityManager
{
    public class Activity
    {
        public int Id { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public double? Duration { get; set; }
        public string? Note { get; set; }

        public Activity(){ Id = 0; }

        public Activity(int id)
        {
            Id = id;
            StartTime = DateTime.Now.ToString();
            EndTime = string.Empty;
            Duration = 0;
            Note = string.Empty;
        }

        public sealed override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"{Id}: {StartTime} - {EndTime} ({Duration}) {Note}");
            return stringBuilder.ToString();
        }

        public void StopActivity()
        {
            var endTime = DateTime.Now;
            var startTime = Convert.ToDateTime(StartTime);
            EndTime = endTime.ToString();
            Duration = Math.Round((endTime - startTime).TotalMinutes, 2);
        }

        public void AddNote(string note)
        {
            Note = note;
        }
    }

    public record ActivityType(int Id, string Name, Activity[] Activities)
    {
        public sealed override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Id: {Id} - {Name}, {Activities.Length} records");
            return stringBuilder.ToString();
        }
    }
}
