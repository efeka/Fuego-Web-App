using Data.Repository;
using Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Models;
using System.Linq.Expressions;

namespace Services
{
    public class InstructorService : IGenericService<Instructor>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly string _instructorImagesDir;
        private readonly string _defaultImageFileName;

        public InstructorService(IConfiguration config, IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

            _instructorImagesDir = config["RelativeWWWImagePaths:InstructorImagesDir"]
                ?? throw new MissingConfigurationException("RelativeWWWImagePaths:InstructorImagesDir");

            _defaultImageFileName = config["RelativeWWWImagePaths:DefaultImageFileName"]
                ?? throw new MissingConfigurationException("RelativeWWWImagePaths:DefaultImageFileName");
        }

        public async Task<IEnumerable<Instructor>> GetAllAsync(string? includeProperties = null)
        {
            return await _unitOfWork.Instructor.GetAllAsync(includeProperties);
        }

        public async Task<Instructor?> GetAsync(Expression<Func<Instructor, bool>> filter, string? includeProperties = null)
        {
            return await _unitOfWork.Instructor.GetAsync(filter, includeProperties);
        }

        public async Task AddAsync(Instructor entity)
        {
            await UpsertAsync(entity, null);
        }

        public async Task UpdateAsync(Instructor entity)
        {
            await UpsertAsync(entity, null);
        }

        public async Task UpsertAsync(Instructor entity, IFormFile? file)
        {
            if (entity.LastPayment == null)
                entity.LastPayment = DateTime.MinValue;

            // Insert
            if (entity.Id == 0)
            {
                AddNewOrDefaultImage(entity, file);
                _unitOfWork.Instructor.Add(entity);
                await _unitOfWork.SaveChangesAsync();
            }
            // Update
            else
            {
                // If a new image file was provided, delete the old image first
                if (file != null)
                {
                    DeleteImageFile(entity.ImageUrl);
                    AddNewOrDefaultImage(entity, file);
                }

                _unitOfWork.Instructor.Update(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            Instructor? instructorToBeDeleted = await _unitOfWork.Instructor.GetAsync(e => e.Id == id);
            if (instructorToBeDeleted == null)
                throw new EntityNotFoundException(id);

            DeleteImageFile(instructorToBeDeleted.ImageUrl);
            _unitOfWork.Instructor.Delete(instructorToBeDeleted);
            await _unitOfWork.SaveChangesAsync();
        }

        private void DeleteImageFile(string imageUrl)
        {
            string defaultImagePath = Path.Combine("\\", _instructorImagesDir, _defaultImageFileName);
            if (imageUrl != defaultImagePath)
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                    System.IO.File.Delete(oldImagePath);
            }
        }

        private void AddNewOrDefaultImage(Instructor entity, IFormFile? file)
        {
            string imageFileName;
            string imagesDir = Path.Combine(_webHostEnvironment.WebRootPath, _instructorImagesDir);

            if (file != null)
                imageFileName = SaveImage(file, imagesDir);
            else
                imageFileName = _defaultImageFileName;

            entity.ImageUrl = Path.Combine("\\", _instructorImagesDir, imageFileName);
        }

        private string SaveImage(IFormFile file, string imagesDir)
        {
            string imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string imagePath = Path.Combine(imagesDir, imageFileName);

            using var fileStream = new FileStream(imagePath, FileMode.Create);
            file.CopyTo(fileStream);

            return imageFileName;
        }
    }
}
