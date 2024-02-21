using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Models.ViewModels
{
    public class CourseUserVM
    {
        public CourseUser CourseUser { get; set; } = new();

        [ValidateNever]
        public IEnumerable<SelectListItem> Courses { get; set; } = Enumerable.Empty<SelectListItem>();

        [ValidateNever]
        public IEnumerable<SelectListItem> ApplicationUsers { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
