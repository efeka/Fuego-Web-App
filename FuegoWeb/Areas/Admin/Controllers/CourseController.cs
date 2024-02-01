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
    public class CourseController : Controller
    {
        private readonly CourseService _courseService;
        private readonly CourseTypeService _courseTypeService;
        private readonly InstructorService _instructorService;

        public CourseController(CourseService courseService, CourseTypeService courseTypeService, InstructorService instructorService)
        {
            _courseService = courseService;
            _courseTypeService = courseTypeService;
            _instructorService = instructorService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Course> courses = await _courseService.GetAllAsync(
                includeProperties: "Instructor,CourseType"
            );
            return View(courses);
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Course? course;
            // Insert
            if (id == null || id == 0)
                course = new Course();
            // Update
            else
            {
                course = await _courseService.GetAsync(u => u.Id == id, "Schedules");
                if (course == null)
                    return View("Error", new ErrorViewModel()
                    {
                        ErrorMessage = "Failed to retrieve Course for update"
                    });
            }

            CourseVM courseVM = new()
            {
                Course = course,
                Instructors = await GetInstructorListAsync(),
                CourseTypes = await GetCourseTypeListAsync()
            };
            return View(courseVM);
        }

        [HttpPost]
        public async Task<ActionResult> Upsert(CourseVM courseVM, IFormFile? file)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    courseVM.CourseTypes = await GetCourseTypeListAsync();
                    courseVM.Instructors = await GetInstructorListAsync();
                    return View(courseVM);
                }

                if (!ImageHandler.IsImageFileValid(file))
                {
                    TempData["error"] = "Invalid image file.";
                    courseVM.CourseTypes = await GetCourseTypeListAsync();
                    courseVM.Instructors = await GetInstructorListAsync();
                    return View(courseVM);
                }

                await _courseService.UpsertAsync(courseVM.Course, file);
                TempData["success"] = courseVM.Course.Id == 0 ?
                    "Course created successfully" :
                    "Course updated successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View("Error", new ErrorViewModel()
                {
                    ErrorMessage = "Failed to upsert Course"
                });
            }
        }

        private async Task<IEnumerable<SelectListItem>> GetCourseTypeListAsync()
        {
            IEnumerable<SelectListItem> courseTypeList =
                (await _courseTypeService.GetAllAsync())
                .Select(x => new SelectListItem
                {
                    Text = x.Type,
                    Value = x.Id.ToString()
                });
            return courseTypeList;
        }

        private async Task<IEnumerable<SelectListItem>> GetInstructorListAsync()
        {
            IEnumerable<SelectListItem> instructorList =
                (await _instructorService.GetAllAsync())
                .Select(x => new SelectListItem
                {
                    Text = x.Name + " " + x.Surname,
                    Value = x.Id.ToString()
                });
            return instructorList;
        }

        #region API Calls

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Course> list = await _courseService.GetAllAsync();
            return Json(new { data = list });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _courseService.DeleteAsync(id);
                return Json(new { success = true, message = $"Course deleted successfully", deletedId = id });
            }
            catch (EntityNotFoundException)
            {
                return Json(new { success = false, message = $"Could not find Course with id {id}" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = $"Could not delete Course with id {id}" });
            }
        }

        #endregion
    }
}
