using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Models.ViewModels
{
    public class InstructorVM
    {
        public Instructor Instructor { get; set; } = new();

        [ValidateNever]
        public IEnumerable<Course> Courses { get; set; } = Enumerable.Empty<Course>();
    }
}
