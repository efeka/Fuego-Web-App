using FuegoWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace FuegoWeb.Areas.Student.Controllers
{
    [Area("Public")]
    public class CourseController : Controller
    {
        private readonly CourseService _courseService;

        public CourseController(CourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Course> courses = await _courseService.GetAllAsync(
                includeProperties: "Instructor,CourseType"
            );
            return View(courses);
        }

        public async Task<IActionResult> Details(int courseId)
        {
            Course? courseFromDb = await _courseService.GetAsync(
                u => u.Id == courseId,
                includeProperties: "Instructor,CourseType"
            );
            if (courseFromDb == null)
            {
                return View("Error", new ErrorViewModel()
                {
                    ErrorMessage = $"Failed to retrieve details for the course with id {courseId}"
                });
            }
            return View("Details", courseFromDb);
        }
    }
}
