using System.ComponentModel.DataAnnotations;

namespace ActivityManager.Web.Models
{
    public class Activity
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double? Duration { get; set; }
        public string? Note { get; set; }
    }
}
