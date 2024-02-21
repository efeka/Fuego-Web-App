using FuegoWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Services;

namespace FuegoWeb.Areas.Student.Controllers
{
    [Area("Public")]
    public class CourseController : Controller
    {
        private readonly CourseService _courseService;
        private readonly ScheduleService _scheduleService;

        public CourseController(CourseService courseService, ScheduleService scheduleService)
        {
            _courseService = courseService;
            _scheduleService = scheduleService;
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
                includeProperties: "Instructor,CourseType,Schedules"
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

        private async Task<IEnumerable<SelectListItem>> GetScheduleListAsync(int courseId)
        {
            IEnumerable<SelectListItem> scheduleList =
                (await _scheduleService.GetAllByCourseIdAsync(courseId))
                .Select(x => new SelectListItem
                {
                    Text = x.DayOfWeek + " " + x.Hour,
                    Value = x.ScheduleId.ToString()
                });
            return scheduleList;
        }
    }
}
