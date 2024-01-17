using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Models.ViewModels
{
    public class CourseStudentVM
    {
        public CourseStudent CourseStudent { get; set; } = new();

        [ValidateNever]
        public Course Course { get; set; } = new();

        [ValidateNever]
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
