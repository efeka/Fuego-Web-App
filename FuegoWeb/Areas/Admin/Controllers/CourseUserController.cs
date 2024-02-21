using Exceptions;
using FuegoWeb.Models;
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
    public class CourseUserController : Controller
    {
        private CourseUserService _courseUserService;
        private CourseService _courseService;
        private ApplicationUserService _applicationUserService;

        public CourseUserController(CourseUserService courseUserService, CourseService courseService, ApplicationUserService applicationUserService)
        {
            _courseUserService = courseUserService;
            _courseService = courseService;
            _applicationUserService = applicationUserService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<CourseUser> courseUsers = await _courseUserService.GetAllAsync(
                includeProperties: "Course,ApplicationUser"
            );
            return View(courseUsers);
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            CourseUser? courseUser;
            // Insert
            if (id == null || id == 0)
                courseUser = new();
            // Update
            else
            {
                courseUser = await _courseUserService.GetAsync(u => u.Id == id, "Course,ApplicationUser");
                if (courseUser == null)
                    return View("Error", new ErrorViewModel()
                    {
                        ErrorMessage = "Failed to retrieve CourseUser for update"
                    });
            }

            CourseUserVM courseUserVM = new()
            {
                CourseUser = courseUser,
                Courses = await GetCourseListAsync(),
                ApplicationUsers = GetAppUserListAsync()
            };
            return View(courseUserVM);
        }

        [HttpPost]
        public async Task<ActionResult> Upsert(CourseUserVM courseUserVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    courseUserVM.Courses = await GetCourseListAsync();
                    courseUserVM.ApplicationUsers = GetAppUserListAsync();
                    return View(courseUserVM);
                }

                await _courseUserService.UpsertAsync(courseUserVM.CourseUser);
                TempData["success"] = courseUserVM.CourseUser.Id == 0 ?
                    "CourseUser created successfully" :
                    "CourseUser updated successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View("Error", new ErrorViewModel()
                {
                    ErrorMessage = "Failed to upsert CourseUser"
                });
            }
        }

        private async Task<IEnumerable<SelectListItem>> GetCourseListAsync()
        {
            IEnumerable<SelectListItem> courseList =
                (await _courseService.GetAllAsync())
                .Select(x => new SelectListItem
                {
                    Text = x.Title,
                    Value = x.Id.ToString()
                });
            return courseList;
        }

        private IEnumerable<SelectListItem> GetAppUserListAsync()
        {
            IEnumerable<SelectListItem> userList =
                _applicationUserService.GetAll()
                .Select(x => new SelectListItem
                {
                    Text = x.Email,
                    Value = x.Id.ToString()
                });
            return userList;
        }

        #region API Calls

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<CourseUser> list = await _courseUserService
                .GetAllAsync(includeProperties: "Course,ApplicationUser");
            return Json(new { data = list });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _courseUserService.DeleteAsync(id);
                return Json(new { success = true, message = $"CourseUser deleted successfully", deletedId = id });
            }
            catch (EntityNotFoundException)
            {
                return Json(new { success = false, message = $"Could not find CourseUser with id {id}" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = $"Could not delete CourseUser with id {id}" });
            }
        }

        #endregion
    }
}
