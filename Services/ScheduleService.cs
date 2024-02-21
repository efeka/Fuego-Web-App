using Data.Repository;
using Exceptions;
using Models;
using System.Linq.Expressions;

namespace Services
{
    public class ScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CourseService _courseService;

        public ScheduleService(IUnitOfWork unitOfWork, CourseService courseService)
        {
            _unitOfWork = unitOfWork;
            _courseService = courseService;
        }

        public async Task<IEnumerable<Schedule>> GetAllAsync(Expression<Func<Schedule, bool>>? filter = null, string? includeProperties = null)
        {
            return await _unitOfWork.Schedule.GetAllAsync(filter, includeProperties);
        }

        public async Task<Schedule?> GetAsync(Expression<Func<Schedule, bool>> filter, string? includeProperties = null)
        {
            return await _unitOfWork.Schedule.GetAsync(filter, includeProperties);
        }

        public async Task<IEnumerable<Schedule>> GetAllByCourseIdAsync(int courseId)
        {
            return await _unitOfWork.Schedule.GetAllAsync(u => u.CourseId == courseId);
        }

        public async Task AddAsync(Schedule entity)
        {
            await UpsertAsync(entity);
        }

        public async Task UpdateAsync(Schedule entity)
        {
            await UpsertAsync(entity);
        }

        public async Task UpsertAsync(Schedule entity)
        {
            #region Include Course navigation property

            Course? courseFromDb = await _courseService.GetAsync(u => u.Id == entity.CourseId);
            if (entity.CourseId == 0 || courseFromDb == null)
                throw new EntityNotFoundException(entity.CourseId);

            entity.Course = courseFromDb;
            entity.CourseId = courseFromDb.Id;

            #endregion

            // Insert
            if (entity.ScheduleId == 0)
            {
                _unitOfWork.Schedule.Add(entity);
                await _unitOfWork.SaveChangesAsync();
            }
            // Update
            else
            {
                _unitOfWork.Schedule.Update(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            Schedule? scheduleToBeDeleted = await _unitOfWork.Schedule.GetAsync(u => u.ScheduleId == id);
            if (scheduleToBeDeleted == null)
                throw new EntityNotFoundException(id);

            _unitOfWork.Schedule.Delete(scheduleToBeDeleted);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
