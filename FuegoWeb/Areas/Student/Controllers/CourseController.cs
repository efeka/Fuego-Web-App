﻿using FuegoWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace FuegoWeb.Areas.Student.Controllers
{
    [Area("Student")]
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

        public async Task<IActionResult> Details(int courseId)
        {
            Course? courseFromDb = await _courseService.GetAsync(
                u => u.Id == courseId,
                includeProperties: "Instructor,CourseType"
            );
            if (courseFromDb == null)
            {
                return View("Error", new ErrorViewModel()
                {
                    ErrorMessage = $"Failed to retrieve details for the course with id {courseId}"
                });
            }
            return View("Details", courseFromDb);
        }
    }
}
