using Data.Repository;
using Exceptions;
using Models;
using System.Linq.Expressions;

namespace Services
{
    public class CourseUserService : IGenericService<CourseUser>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CourseService _courseService;
        private readonly ApplicationUserService _userService;

        public CourseUserService(IUnitOfWork unitOfWork, CourseService courseService, ApplicationUserService userService)
        {
            _unitOfWork = unitOfWork;
            _courseService = courseService;
            _userService = userService;
        }

        public async Task<IEnumerable<CourseUser>> GetAllAsync(Expression<Func<CourseUser, bool>>? filter = null, string? includeProperties = null)
        {
            return await _unitOfWork.CourseUser.GetAllAsync(filter, includeProperties);
        }

        public async Task<CourseUser?> GetAsync(Expression<Func<CourseUser, bool>> filter, string? includeProperties = null)
        {
            return await _unitOfWork.CourseUser.GetAsync(filter, includeProperties);
        }

        public async Task AddAsync(CourseUser entity)
        {
            await UpsertAsync(entity);
        }

        public async Task UpdateAsync(CourseUser entity)
        {
            await UpsertAsync(entity);
        }

        public async Task UpsertAsync(CourseUser entity)
        {
            // TODO Get eager loading to work
            #region Include Course and ApplicationUser navigation properties

            Course? courseFromDb = await _courseService.GetAsync(u => u.Id == entity.CourseId);
            if (entity.CourseId == 0 || courseFromDb == null)
                throw new EntityNotFoundException(entity.CourseId);

            ApplicationUser? userFromDb = _userService.Get(entity.ApplicationUserId);
            if (string.IsNullOrEmpty(entity.ApplicationUserId) || userFromDb == null)
                throw new EntityNotFoundException(entity.ApplicationUserId);

            entity.Course = courseFromDb;
            entity.CourseId = courseFromDb.Id;
            entity.ApplicationUser = userFromDb;
            entity.ApplicationUserId = userFromDb.Id;

            #endregion

            // Insert
            if (entity.Id == 0)
            {
                _unitOfWork.CourseUser.Add(entity);
                await _unitOfWork.SaveChangesAsync();
            }
            // Update
            else
            {
                _unitOfWork.CourseUser.Update(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            CourseUser? cuToBeDeleted = await _unitOfWork.CourseUser.GetAsync(u => u.Id == id);
            if (cuToBeDeleted == null)
                throw new EntityNotFoundException(id);

            _unitOfWork.CourseUser.Delete(cuToBeDeleted);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
