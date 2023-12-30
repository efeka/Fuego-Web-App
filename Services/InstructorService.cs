using Data.Repository;
using Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Linq.Expressions;

namespace Services
{
    public class InstructorService : IGenericService<Instructor>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public InstructorService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
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

        public async Task<Instructor> GetAsync(Expression<Func<Instructor, bool>> filter, string? includeProperties = null)
        {
            Instructor? instructor = await _unitOfWork.Instructor.GetAsync(filter, includeProperties);
            if (instructor == null)
                throw new EntityNotFoundException();
            return instructor;
        }

        public async Task AddAsync(Instructor entity)
        {
            await AddAsync(entity, null);
        }

        public async Task AddAsync(Instructor entity, IFormFile? file)
        {
            try
            {
                HandleInstructorImageFile(entity, file);

                if (entity.LastPayment == null)
                    entity.LastPayment = DateTime.MinValue;

                _unitOfWork.Instructor.Add(entity);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        private void HandleInstructorImageFile(Instructor entity, IFormFile? file)
        {
            string imagesDir = Path.Combine(
                _webHostEnvironment.WebRootPath,
                @"images\instructors");

            string imageFileName;
            // If an image file was uploaded, save it under wwwroot/images/instructors
            if (file != null)
            {
                imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                using var fileStream = new FileStream(Path.Combine(imagesDir, imageFileName), FileMode.Create);
                file.CopyTo(fileStream);
            }
            // If an image file was not provided, assign the default picture to the instructor
            else
                imageFileName = "default_user.png";

            entity.ImageUrl = Path.Combine("/images/instructors/", imageFileName);
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
