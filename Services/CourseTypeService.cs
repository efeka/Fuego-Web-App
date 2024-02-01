using Data.Repository;
using Exceptions;
using Models;
using System.Linq.Expressions;

namespace Services
{
    public class CourseTypeService : IGenericService<CourseType>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CourseType>> GetAllAsync(Expression<Func<CourseType, bool>>? filter = null, string? includeProperties = null)
        {
            return await _unitOfWork.CourseType.GetAllAsync(filter, includeProperties);
        }

        public async Task<CourseType?> GetAsync(Expression<Func<CourseType, bool>> filter, string? includeProperties = null)
        {
            return await _unitOfWork.CourseType.GetAsync(filter, includeProperties);
        }

        public async Task AddAsync(CourseType entity)
        {
            _unitOfWork.CourseType.Add(entity);
            await _unitOfWork.SaveChangesAsync();
            Console.WriteLine(entity);
            Console.WriteLine(entity);
        }

        public async Task UpdateAsync(CourseType entity)
        {
            _unitOfWork.CourseType.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            CourseType? courseTypeToBeDeleted = await _unitOfWork.CourseType.GetAsync(e => e.Id == id);
            if (courseTypeToBeDeleted == null)
                throw new EntityNotFoundException(id);

            _unitOfWork.CourseType.Delete(courseTypeToBeDeleted);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
