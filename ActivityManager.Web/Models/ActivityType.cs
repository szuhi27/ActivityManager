namespace ActivityManager.Web.Models
{
    public class ActivityType
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Activity> Activities { get; } = new List<Activity>(); //1:N

    }
}