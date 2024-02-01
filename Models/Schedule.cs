namespace Models
{
    public class Schedule
    {
        public int ScheduleId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan Hour { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
