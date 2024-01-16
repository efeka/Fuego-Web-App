using FuegoWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels;
using Services;

namespace FuegoWeb.Areas.Public.Controllers
{
    [Area("Public")]
    public class InstructorController : Controller
    {
        private readonly InstructorService _instructorService;
        private readonly CourseService _courseService;

        public InstructorController(InstructorService instructorService, CourseService courseService)
        {
            _instructorService = instructorService;
            _courseService = courseService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Instructor> instructors = await _instructorService.GetAllAsync();
            List<InstructorVM> instructorVMs = new();

            foreach (var instructor in instructors)
            {
                InstructorVM instructorVM = new()
                {
                    Instructor = instructor,
                    Courses = await _courseService.GetAllByInstructorIdAsync(instructor.Id, includeProperties: "CourseType")
                };
                instructorVMs.Add(instructorVM);
            }
            return View(instructorVMs);
        }

        public async Task<IActionResult> Details(int instructorId)
        {
            Instructor? instructorFromDb = await _instructorService.GetAsync(u => u.Id == instructorId);
            if (instructorFromDb == null)
            {
                return View("Error", new ErrorViewModel()
                {
                    ErrorMessage = $"Failed to retrieve details for the instructor with id {instructorId}"
                });
            }

            InstructorVM instructorVM = new()
            {
                Instructor = instructorFromDb,
                Courses = await _courseService.GetAllByInstructorIdAsync(instructorFromDb.Id, includeProperties: "CourseType")
            };
            return View("Details", instructorVM);
        }

    }
}
