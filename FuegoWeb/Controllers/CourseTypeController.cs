using FuegoWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace FuegoWeb.Controllers
{
    public class CourseTypeController : Controller
    {
        private readonly CourseTypeService _courseTypeService;

        public CourseTypeController(CourseTypeService courseTypeService)
        {
            _courseTypeService = courseTypeService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<CourseType> courseTypes = await _courseTypeService.GetAllAsync();
            return View(courseTypes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseType courseType)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                await _courseTypeService.AddAsync(courseType);
                TempData["success"] = "Course Type created successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View("Error", new ErrorViewModel()
                {
                    ErrorMessage = "Failed to create Course Type"
                });
            }
        }

    }
}
