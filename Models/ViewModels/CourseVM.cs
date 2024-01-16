using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Models.ViewModels
{
    public class CourseVM
    {
        public Course Course { get; set; } = new();

        [ValidateNever]
        public IEnumerable<SelectListItem> CourseTypes { get; set; } = Enumerable.Empty<SelectListItem>();

        [ValidateNever]
        public IEnumerable<SelectListItem> Instructors { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
