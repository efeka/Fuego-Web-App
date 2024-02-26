using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using System.Security.Claims;

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

        #region API Calls

        [HttpGet("/Public/CourseUser/GetCredits/{courseId}")]
        public async Task<IActionResult> GetCreditsForCurrentUser(int courseId)
        {
            string? currUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(currUserId))
                return StatusCode(StatusCodes.Status401Unauthorized, new { error = "User not authenticated." });

            CourseUser? cuFromDb = await _courseUserService
                .GetAsync(u => u.CourseId == courseId && u.ApplicationUserId == currUserId);

            int credits = cuFromDb == null ? 0 : cuFromDb.UserCredits;
            return Json(new { credits });
        }

        #endregion
    }
}