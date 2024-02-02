using System.Text.Json.Serialization;

namespace Models
{
    public class Schedule
    {
        public int ScheduleId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan Hour { get; set; }

        [JsonIgnore]
        public int CourseId { get; set; }
        [JsonIgnore]
        public Course Course { get; set; }
    }
}
