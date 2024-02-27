using Data.Repository;
using Exceptions;
using Models;
using System.Linq.Expressions;

namespace Services
{
    public class UserScheduleService : IGenericService<UserSchedule>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationUserService _userService;
        private readonly ScheduleService _scheduleService;

        public UserScheduleService(IUnitOfWork unitOfWork, ApplicationUserService userService, ScheduleService scheduleService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _scheduleService = scheduleService;
        }

        public async Task<IEnumerable<UserSchedule>> GetAllAsync(Expression<Func<UserSchedule, bool>>? filter = null, string? includeProperties = null)
        {
            return await _unitOfWork.UserSchedule.GetAllAsync(filter, includeProperties);
        }

        public async Task<UserSchedule?> GetAsync(Expression<Func<UserSchedule, bool>> filter, string? includeProperties = null)
        {
            return await _unitOfWork.UserSchedule.GetAsync(filter, includeProperties);
        }

        public async Task AddAsync(UserSchedule entity)
        {
            await UpsertAsync(entity);
        }

        public async Task UpdateAsync(UserSchedule entity)
        {
            await UpsertAsync(entity);
        }

        public async Task UpsertAsync(UserSchedule entity)
        {
            #region Include ApplicationUser and Schedule entities

            if (string.IsNullOrEmpty(entity.ApplicationUserId))
                throw new EntityNotFoundException(entity.ApplicationUserId);

            ApplicationUser? userFromDb = _userService.Get(entity.ApplicationUserId);
            if (userFromDb == null)
                throw new EntityNotFoundException(entity.ApplicationUserId);

            Schedule? scheduleFromDb = await _scheduleService.GetAsync(u => u.ScheduleId == entity.ScheduleId);
            if (entity.ScheduleId == 0 || scheduleFromDb == null)
                throw new EntityNotFoundException(entity.ScheduleId);

            entity.ApplicationUser = userFromDb;
            entity.ApplicationUserId = userFromDb.Id;
            entity.Schedule = scheduleFromDb;
            entity.ScheduleId = scheduleFromDb.ScheduleId;

            #endregion

            // Insert
            if (entity.ReservationId == 0)
            {
                _unitOfWork.UserSchedule.Add(entity);
                await _unitOfWork.SaveChangesAsync();
            }
            // Update
            else
            {
                _unitOfWork.UserSchedule.Update(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            UserSchedule? reservationToBeDeleted = await _unitOfWork.UserSchedule.GetAsync(e => e.ReservationId == id);
            if (reservationToBeDeleted == null)
                throw new EntityNotFoundException(id);

            _unitOfWork.UserSchedule.Delete(reservationToBeDeleted);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
