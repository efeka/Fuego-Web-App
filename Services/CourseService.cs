using Data.Repository;
using Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Models;
using System.Linq.Expressions;
using Utility;

namespace Services
{
    public class CourseService : IGenericService<Course>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly InstructorService _instructorService;
        private readonly CourseTypeService _courseTypeService;
        private readonly ImageHandler _imageHandler;

        private readonly string _courseImagesDir;
        private readonly string _defaultImageFileName;
        private readonly string _defaultImagePath;

        public CourseService(IConfiguration config, IUnitOfWork unitOfWork, InstructorService instructorService,
            CourseTypeService courseTypeService, ImageHandler imageHandler)
        {
            _unitOfWork = unitOfWork;
            _instructorService = instructorService;
            _courseTypeService = courseTypeService;
            _imageHandler = imageHandler;

            _courseImagesDir = config["RelativeWWWImagePaths:CourseImagesDir"]
                ?? throw new MissingConfigurationException("RelativeWWWImagePaths:CourseImagesDir");

            _defaultImageFileName = config["RelativeWWWImagePaths:DefaultCourseImageFileName"]
                ?? throw new MissingConfigurationException("RelativeWWWImagePaths:DefaultCourseImageFileName");

            _defaultImagePath = Path.Combine("\\", _courseImagesDir, _defaultImageFileName);
        }

        public async Task<IEnumerable<Course>> GetAllAsync(string? includeProperties = null)
        {
            return await _unitOfWork.Course.GetAllAsync(includeProperties);
        }

        public async Task<Course?> GetAsync(Expression<Func<Course, bool>> filter, string? includeProperties = null)
        {
            return await _unitOfWork.Course.GetAsync(filter, includeProperties);
        }

        public async Task<IEnumerable<Course>> GetAllByInstructorIdAsync(int id, string? includeProperties = null)
        {
            return (await _unitOfWork.Course.GetAllAsync(includeProperties)).Where(u => u.InstructorId == id);
        }

        public async Task AddAsync(Course entity)
        {
            await UpsertAsync(entity, null);
        }

        public async Task UpdateAsync(Course entity)
        {
            await UpsertAsync(entity, null);
        }

        public async Task UpsertAsync(Course entity, IFormFile? file)
        {
            string imageUrl = _imageHandler.GetNewOrDefaultImagePath(file, _courseImagesDir, _defaultImageFileName);

            #region Include Instructor and CourseType entities

            if (entity.InstructorId == 0 || entity.CourseTypeId == 0)
                throw new EntityNotFoundException(entity.Instructor.Id);

            Instructor? instructorFromDb = await _instructorService.GetAsync(u => u.Id == entity.InstructorId);
            CourseType? courseTypeFromDb = await _courseTypeService.GetAsync(u => u.Id == entity.CourseTypeId);
            if (instructorFromDb == null || courseTypeFromDb == null)
                throw new EntityNotFoundException(entity.Instructor.Id);

            entity.Instructor = instructorFromDb;
            entity.InstructorId = instructorFromDb.Id;
            entity.CourseType = courseTypeFromDb;
            entity.CourseTypeId = courseTypeFromDb.Id;

            #endregion

            // Insert
            if (entity.Id == 0)
            {
                entity.ImageUrl = imageUrl;
                if (file != null)
                    _imageHandler.SaveImage(file, imageUrl);

                _unitOfWork.Course.Add(entity);
                await _unitOfWork.SaveChangesAsync();
            }
            // Update
            else
            {
                if (file != null)
                {
                    // Delete old image before creating the new one
                    _imageHandler.DeleteImageFile(entity.ImageUrl, _defaultImagePath);

                    entity.ImageUrl = imageUrl;
                    if (file != null)
                        _imageHandler.SaveImage(file, imageUrl);
                }

                _unitOfWork.Course.Update(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            Course? courseToBeDeleted = await _unitOfWork.Course.GetAsync(e => e.Id == id);
            if (courseToBeDeleted == null)
                throw new EntityNotFoundException(id);

            _imageHandler.DeleteImageFile(courseToBeDeleted.ImageUrl, _defaultImagePath);
            _unitOfWork.Course.Delete(courseToBeDeleted);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
