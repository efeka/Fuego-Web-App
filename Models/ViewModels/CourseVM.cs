using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Models.ViewModels
{
    public class CourseVM
    {
        public Course Course { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CourseTypes { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Instructors { get; set; }
    }
}
