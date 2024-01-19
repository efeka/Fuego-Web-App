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
            return View(courseStudents);
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            CourseStudent? courseStudent;

            // Insert
            if (id == null || id == 0)
            {
                courseStudent = new();
            }
            // Update
            else
            {
                courseStudent = await _csService.GetAsync(u => u.Id == id);
                if (courseStudent == null)
                    return View("Error", new ErrorViewModel()
                    {
                        ErrorMessage = "Failed to retrieve CourseStudent for update"
                    });
            }

            CourseStudentVM csVM = new()
            {
                CourseStudent = courseStudent,
                Courses = await GetCoursesSelectListAsync(),
                ApplicationUsers = GetAppUsersSelectListAsync()
            };
            return View(csVM);
        }

        [HttpPost]
        public async Task<ActionResult> Upsert(CourseStudentVM csVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    csVM.Courses = await GetCoursesSelectListAsync();
                    csVM.ApplicationUsers = GetAppUsersSelectListAsync();
                    return View(csVM);
                }

                await _csService.UpsertAsync(csVM.CourseStudent);

                TempData["success"] = csVM.CourseStudent.Id == 0 ?
                    "Course-Student created successfully" :
                    "Course-Student updated successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View("Error", new ErrorViewModel()
                {
                    ErrorMessage = "Failed to upsert CourseStudent"
                });
            }
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

        #region API Calls

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _csService.DeleteAsync(id);
                return Json(new { success = true, message = $"CourseStudent deleted successfully", deletedId = id });
            }
            catch (EntityNotFoundException)
            {
                return Json(new { success = false, message = $"Could not find CourseStudent with id {id}" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = $"Could not delete CourseStudent with id {id}" });
            }
        }

        #endregion
    }
}
