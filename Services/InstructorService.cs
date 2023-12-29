using Data.Repository;
using Exceptions;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Linq.Expressions;

namespace Services
{
    public class InstructorService : IGenericService<Instructor>
    {
        private readonly IUnitOfWork _unitOfWork;

        public InstructorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<JsonResult> GetApiDataAsync()
        {
            IEnumerable<Instructor> instructors = await _unitOfWork.Instructor.GetAllAsync();
            return new JsonResult(instructors);
        }

        public async Task<IEnumerable<Instructor>> GetAllAsync(string? includeProperties = null)
        {
            return await _unitOfWork.Instructor.GetAllAsync(includeProperties);
        }

        public async Task<Instructor?> GetAsync(Expression<Func<Instructor, bool>> filter, string? includeProperties = null)
        {
            Instructor? instructor = await _unitOfWork.Instructor.GetAsync(filter, includeProperties);
            if (instructor == null)
                throw new EntityNotFoundException();
            return instructor;
        }

        public async Task AddAsync(Instructor entity)
        {
            try
            {
                _unitOfWork.Instructor.Add(entity);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            Instructor? instructorToBeDeleted = await _unitOfWork.Instructor.GetAsync(e => e.Id == id);
            if (instructorToBeDeleted == null)
                throw new EntityNotFoundException(id);

            _unitOfWork.Instructor.Delete(instructorToBeDeleted);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Instructor entity)
        {
            Instructor? instructorToBeUpdated = await _unitOfWork.Instructor.GetAsync(e => e.Id == entity.Id);
            if (instructorToBeUpdated == null)
                throw new EntityNotFoundException(entity.Id);

            _unitOfWork.Instructor.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
