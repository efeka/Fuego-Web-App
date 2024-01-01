using Exceptions;
using FuegoWeb.Models;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Instructor instructor, IFormFile? file)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                await _instructorService.UpsertAsync(instructor, file);
                TempData["success"] = "Instructor created successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View("Error", new ErrorViewModel()
                {
                    ErrorMessage = "Failed to create Instructor"
                });
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                Instructor? instructorFromDb = await _instructorService.GetAsync(u => u.Id == id);
                if (instructorFromDb == null)
                    throw new EntityNotFoundException(id);

                return View(instructorFromDb);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View("Error", new ErrorViewModel()
                {
                    ErrorMessage = "Failed to edit Instructor"
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Instructor instructor, IFormFile? file)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                await _instructorService.UpsertAsync(instructor, file);
                TempData["success"] = "Instructor edited successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View("Error", new ErrorViewModel()
                {
                    ErrorMessage = "Failed to edit Instructor"
                });
            }
        }

        #region API Calls

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Instructor> list = await _instructorService.GetAllAsync();
            return Json(new { data = list });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _instructorService.DeleteAsync(id);
                return Json(new { success = true, message = $"Instructor deleted successfully", deletedId = id });
            }
            catch (EntityNotFoundException)
            {
                return Json(new { success = false, message = $"Could not find Instructor with id {id}" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = $"Could not delete Instructor with id {id}" });
            }
        }

        #endregion
    }
}
