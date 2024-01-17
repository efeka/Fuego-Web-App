using Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using Utility;

namespace FuegoWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ApplicationRoles.Role_Admin)]
    public class ApplicationUserController : Controller
    {
        private ApplicationUserService _userService;

        public ApplicationUserController(ApplicationUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            List<ApplicationUser> appUsers = _userService.GetAll();
            return View(appUsers);
        }

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<ApplicationUser> list = _userService.GetAll();
            return Json(new { data = list });
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            try
            {
                _userService.DeleteAsync(id);
                return Json(new { success = true, message = $"User deleted successfully", deletedId = id });
            }
            catch (EntityNotFoundException)
            {
                return Json(new { success = false, message = $"Could not find User with id {id}" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = $"Could not delete User with id {id}" });
            }
        }

        #endregion
    }
}
