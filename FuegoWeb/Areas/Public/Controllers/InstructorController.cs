using FuegoWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace FuegoWeb.Areas.Public.Controllers
{
    [Area("Public")]
    public class InstructorController : Controller
    {
        private readonly InstructorService _instructorService;

        public InstructorController(InstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Instructor> instructors = await _instructorService.GetAllAsync();
            return View(instructors);
        }

        public async Task<IActionResult> Details(int instructorId)
        {
            Instructor? instructorFromDb = await _instructorService.GetAsync(
                u => u.Id == instructorId
            );
            if (instructorFromDb == null)
            {
                return View("Error", new ErrorViewModel()
                {
                    ErrorMessage = $"Failed to retrieve details for the instructor with id {instructorId}"
                });
            }
            return View("Details", instructorFromDb);
        }

    }
}
