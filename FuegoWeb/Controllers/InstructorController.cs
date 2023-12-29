using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace FuegoWeb.Controllers
{
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

    }
}
