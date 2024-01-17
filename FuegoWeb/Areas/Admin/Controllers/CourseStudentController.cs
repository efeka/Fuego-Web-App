using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.ViewModels;
using Services;
using Utility;

namespace FuegoWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ApplicationRoles.Role_Admin)]
    public class CourseStudentController : Controller
    {
        private readonly CourseStudentService _csService;
        private readonly CourseService _courseService;
        private readonly ApplicationUserService _appUserService;

        public CourseStudentController(CourseStudentService courseStudentService, CourseService courseService, ApplicationUserService applicationUserService)
        {
            _csService = courseStudentService;
            _courseService = courseService;
            _appUserService = applicationUserService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<CourseStudent> courseStudents = await _csService.GetAllAsync(
                includeProperties: "Course,ApplicationUser"
            );

            List<CourseStudentVM> courseStudentVMs = new();
            IEnumerable<SelectListItem> courseSelectList = await GetCoursesSelectListAsync();
            IEnumerable<SelectListItem> userSelectList = GetAppUsersSelectListAsync();

            foreach (var courseStudent in courseStudents)
            {
                CourseStudentVM courseStudentVM = new()
                {
                    CourseStudent = courseStudent,
                    Courses = courseSelectList,
                    ApplicationUsers = userSelectList
                };
                courseStudentVMs.Add(courseStudentVM);
            }

            return View(courseStudentVMs);
        }

        private async Task<IEnumerable<SelectListItem>> GetCoursesSelectListAsync()
        {
            IEnumerable<SelectListItem> courseList =
                (await _courseService.GetAllAsync())
                .Select(u => new SelectListItem
                {
                    Text = u.Title,
                    Value = u.Id.ToString()
                });
            return courseList;
        }

        private IEnumerable<SelectListItem> GetAppUsersSelectListAsync()
        {
            IEnumerable<SelectListItem> userList =
                _appUserService.GetAll()
                .Select(u => new SelectListItem
                {
                    Text = u.Email,
                    Value = u.Id.ToString()
                });
            return userList;
        }
    }
}
