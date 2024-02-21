using Microsoft.AspNetCore.Mvc;

namespace FuegoWeb.Areas.Public.Controllers
{
    [Area("Public")]
    public class UserScheduleController : Controller
    {
        public IActionResult AddUserSchedule(int courseId, int scheduleId)
        {
            return Ok();
        }
    }
}
