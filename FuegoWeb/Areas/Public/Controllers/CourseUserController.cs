using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using Services;

namespace FuegoWeb.Areas.Public.Controllers
{
    [Area("Public")]
    public class CourseUserController : Controller
    {
        private readonly CourseUserService _courseUserService;

        public CourseUserController(CourseUserService courseUserService)
        {
            _courseUserService = courseUserService;
        }

        public IActionResult ReserveCourse(CourseUserVM reservation)
        {
            return Ok();
        }
    }
}
