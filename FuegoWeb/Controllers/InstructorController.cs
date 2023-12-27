using Data.Repository;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace FuegoWeb.Controllers
{
    public class InstructorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public InstructorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Instructor> instructors = await _unitOfWork.Instructor.GetAllAsync();
            return View(instructors);
        }

    }
}
