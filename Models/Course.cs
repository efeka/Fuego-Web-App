using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [ValidateNever]
        [ForeignKey("CourseTypeId")]
        public CourseType CourseType { get; set; }
        public int CourseTypeId { get; set; }

        [ValidateNever]
        [ForeignKey("InstructorId")]
        public Instructor Instructor { get; set; }
        public int InstructorId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [ValidateNever]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        public List<DayOfWeek> DaysOfWeek { get; set; } = new();
    }
}
