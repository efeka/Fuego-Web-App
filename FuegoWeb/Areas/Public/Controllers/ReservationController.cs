using Exceptions;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace FuegoWeb.Areas.Public.Controllers
{
    [Area("Public")]
    public class ReservationController : Controller
    {
        private readonly ReservationService _reservationService;
        private readonly CourseService _courseService;

        public ReservationController(ReservationService reservationService, CourseService courseService)
        {
            _reservationService = reservationService;
            _courseService = courseService;
        }

        #region API Calls

        // Expects the date string in the yyyy-MM-dd format
        [HttpGet("/Public/Reservation/GetReservationCounts/{courseId}/{date}")]
        public async Task<IActionResult> GetReservationCounts(int courseId, string date)
        {
            try
            {
                DateOnly parsedDate = DateOnly.ParseExact(date, "yyyy-MM-dd", null);
                Dictionary<TimeOnly, int> reservationMap =
                    await _reservationService.GetReservationsPerHourAsync(courseId, parsedDate);

                return Ok(reservationMap);
            }
            catch (FormatException)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { error = "Could not parse the given date due to invalid format." });
            }
            catch (EntityNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { error = ex.Message });
            }
        }

        #endregion
    }
}