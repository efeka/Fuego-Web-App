using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class UserScheduleVM
    {
        public int CourseId { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public TimeSpan Time { get; set; }
    }
}
