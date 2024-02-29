using Data.Repository;
using Exceptions;
using Models;
using System.Linq.Expressions;

namespace Services
{
    public class ReservationService : IGenericService<Reservation>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationUserService _userService;
        private readonly ScheduleService _scheduleService;
        private readonly CourseService _courseService;

        public ReservationService(IUnitOfWork unitOfWork, ApplicationUserService userService, ScheduleService scheduleService, CourseService courseService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _scheduleService = scheduleService;
            _courseService = courseService;
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync(Expression<Func<Reservation, bool>>? filter = null, string? includeProperties = null)
        {
            return await _unitOfWork.Reservation.GetAllAsync(filter, includeProperties);
        }

        public async Task<Reservation?> GetAsync(Expression<Func<Reservation, bool>> filter, string? includeProperties = null)
        {
            return await _unitOfWork.Reservation.GetAsync(filter, includeProperties);
        }

        public async Task<Dictionary<TimeOnly, int>> GetReservationsPerHourAsync(int courseId, DateOnly date)
        {
            Course? courseFromDb = await _courseService.GetAsync(u => u.Id == courseId);
            if (courseFromDb == null)
                throw new EntityNotFoundException(courseId);

            IEnumerable<Reservation> allReservations = await GetAllAsync(
                filter: u => u.CourseId == courseId
            );
            IEnumerable<Reservation> reservations = allReservations
                .Where(u => u.Date == date)
                .ToList();

            Dictionary<TimeOnly, int> resMap = new();
            foreach (Reservation reservation in reservations)
            {
                TimeOnly hour = reservation.Time;
                if (resMap.TryGetValue(hour, out int count))
                    resMap[hour] = count + 1;
                else
                    resMap.TryAdd(hour, 1);
            }

            return resMap;
        }

        public async Task AddAsync(Reservation entity)
        {
            await UpsertAsync(entity);
        }

        public async Task UpdateAsync(Reservation entity)
        {
            await UpsertAsync(entity);
        }

        public async Task UpsertAsync(Reservation entity)
        {
            #region Include ApplicationUser and Course entities

            if (string.IsNullOrEmpty(entity.ApplicationUserId))
                throw new EntityNotFoundException(entity.ApplicationUserId);

            ApplicationUser? userFromDb = _userService.Get(entity.ApplicationUserId);
            if (userFromDb == null)
                throw new EntityNotFoundException(entity.ApplicationUserId);

            Course? courseFromDb = await _courseService.GetAsync(u => u.Id == entity.CourseId);
            if (entity.CourseId == 0 || courseFromDb == null)
                throw new EntityNotFoundException(entity.CourseId);

            entity.ApplicationUser = userFromDb;
            entity.ApplicationUserId = userFromDb.Id;
            entity.Course = courseFromDb;
            entity.CourseId = courseFromDb.Id;

            #endregion

            // Verify that the given date-time pair is valid for the schedule of the course
            Schedule? scheduleFromDb = await _scheduleService.GetAsync(filter: u =>
                u.DayOfWeek == entity.Date.DayOfWeek &&
                u.Hour == entity.Time
            );
            if (scheduleFromDb == null)
                throw new EntityNotFoundException("Could not match Reservation date & time with an existing Schedule.");

            // Insert
            if (entity.ReservationId == 0)
            {
                _unitOfWork.Reservation.Add(entity);
                await _unitOfWork.SaveChangesAsync();
            }
            // Update
            else
            {
                _unitOfWork.Reservation.Update(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            Reservation? reservationToBeDeleted = await _unitOfWork.Reservation.GetAsync(e => e.ReservationId == id);
            if (reservationToBeDeleted == null)
                throw new EntityNotFoundException(id);

            _unitOfWork.Reservation.Delete(reservationToBeDeleted);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
