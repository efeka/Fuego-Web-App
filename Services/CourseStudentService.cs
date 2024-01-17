using Data.Repository;
using Exceptions;
using Models;
using System.Linq.Expressions;

namespace Services
{
    public class CourseStudentService : IGenericService<CourseStudent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CourseService _courseService;
        private readonly ApplicationUserService _appUserService;

        public CourseStudentService(IUnitOfWork unitOfWork, CourseService courseService, ApplicationUserService appUserService)
        {
            _unitOfWork = unitOfWork;
            _courseService = courseService;
            _appUserService = appUserService;
        }

        public async Task<IEnumerable<CourseStudent>> GetAllAsync(string? includeProperties = null)
        {
            return await _unitOfWork.CourseStudent.GetAllAsync(includeProperties);
        }

        public async Task<CourseStudent?> GetAsync(Expression<Func<CourseStudent, bool>> filter, string? includeProperties = null)
        {
            return await _unitOfWork.CourseStudent.GetAsync(filter, includeProperties);
        }

        public async Task AddAsync(CourseStudent entity)
        {
            await UpsertAsync(entity);
        }

        public async Task UpdateAsync(CourseStudent entity)
        {
            await UpsertAsync(entity);
        }

        public async Task UpsertAsync(CourseStudent entity)
        {
            Course? course = await _courseService.GetAsync(u => u.Id == entity.CourseId);
            if (course == null)
                throw new EntityNotFoundException($"Could not find Course with ID {entity.CourseId}");

            ApplicationUser? appUser = _appUserService.Get(entity.ApplicationUserId);
            if (appUser == null)
                throw new EntityNotFoundException($"Could not find User with ID {entity.ApplicationUserId}");

            entity.Course = course;
            entity.ApplicationUser = appUser;

            // Insert
            if (entity.Id == 0)
            {
                _unitOfWork.CourseStudent.Add(entity);
                await _unitOfWork.SaveChangesAsync();
            }
            // Update
            else
            {
                CourseStudent? csFromDb = await _unitOfWork.CourseStudent.GetAsync(u => u.Id == entity.Id);
                if (csFromDb == null)
                    throw new EntityNotFoundException(entity.Id);

                _unitOfWork.CourseStudent.Update(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            CourseStudent? csFromDb = await _unitOfWork.CourseStudent.GetAsync(u => u.Id == id);
            if (csFromDb == null)
                throw new EntityNotFoundException(id);

            _unitOfWork.CourseStudent.Delete(csFromDb);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
