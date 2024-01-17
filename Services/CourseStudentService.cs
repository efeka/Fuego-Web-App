using Data.Repository;
using Exceptions;
using Models;
using System.Linq.Expressions;

namespace Services
{
    public class CourseStudentService : IGenericService<CourseStudent>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseStudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            _unitOfWork.CourseStudent.Add(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(CourseStudent entity)
        {
            CourseStudent? csFromDb = await _unitOfWork.CourseStudent.GetAsync(u => u.Id == entity.Id);
            if (csFromDb == null)
                throw new EntityNotFoundException(entity.Id);

            _unitOfWork.CourseStudent.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            CourseStudent? csFromDb = await _unitOfWork.CourseStudent.GetAsync(u => u.Id == entity.Id);
            if (csFromDb == null)
                throw new EntityNotFoundException(id);

            _unitOfWork.CourseStudent.Delete(csFromDb);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
