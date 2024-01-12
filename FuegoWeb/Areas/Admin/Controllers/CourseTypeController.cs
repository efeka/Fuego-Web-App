using Exceptions;
using FuegoWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using Utility;

namespace FuegoWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ApplicationRoles.Role_Admin)]
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

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                CourseType? courseTypeFromDb = await _courseTypeService.GetAsync(u => u.Id == id);
                if (courseTypeFromDb == null)
                    throw new EntityNotFoundException(id);

                return View(courseTypeFromDb);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View("Error", new ErrorViewModel()
                {
                    ErrorMessage = "Failed to edit Course Type"
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CourseType courseType)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                await _courseTypeService.UpdateAsync(courseType);
                TempData["success"] = "Course Type edited successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View("Error", new ErrorViewModel()
                {
                    ErrorMessage = "Failed to edit Course Type"
                });
            }
        }

        #region API Calls

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<CourseType> list = await _courseTypeService.GetAllAsync();
            return Json(new { data = list });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _courseTypeService.DeleteAsync(id);
                return Json(new { success = true, message = $"Course Type deleted successfully", deletedId = id });
            }
            catch (EntityNotFoundException)
            {
                return Json(new { success = false, message = $"Could not find Course Type with id {id}" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = $"Could not delete Course Type with id {id}" });
            }
        }

        #endregion
    }
}
