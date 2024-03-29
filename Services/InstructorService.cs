﻿using Data.Repository;
using Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Models;
using System.Linq.Expressions;
using Utility;

namespace Services
{
    public class InstructorService : IGenericService<Instructor>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ImageHandler _imageHandler;

        private readonly string _instructorImagesDir;
        private readonly string _defaultInstructorImageFileName;
        private readonly string _defaultImagePath;

        public InstructorService(IConfiguration config, IUnitOfWork unitOfWork, ImageHandler imageHandler)
        {
            _unitOfWork = unitOfWork;
            _imageHandler = imageHandler;

            _instructorImagesDir = config["RelativeWWWImagePaths:InstructorImagesDir"]
                ?? throw new MissingConfigurationException("RelativeWWWImagePaths:InstructorImagesDir");

            _defaultInstructorImageFileName = config["RelativeWWWImagePaths:DefaultInstructorImageFileName"]
                ?? throw new MissingConfigurationException("RelativeWWWImagePaths:DefaultInstructorImageFileName");

            _defaultImagePath = Path.Combine("\\", _instructorImagesDir, _defaultInstructorImageFileName);
        }

        public async Task<IEnumerable<Instructor>> GetAllAsync(Expression<Func<Instructor, bool>>? filter = null, string? includeProperties = null)
        {
            return await _unitOfWork.Instructor.GetAllAsync(filter, includeProperties);
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

            string imageUrl = _imageHandler.GetNewOrDefaultImagePath(file, _instructorImagesDir, _defaultInstructorImageFileName);

            // Insert
            if (entity.Id == 0)
            {
                entity.ImageUrl = imageUrl;
                if (file != null)
                    _imageHandler.SaveImage(file, imageUrl);

                _unitOfWork.Instructor.Add(entity);
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

                _unitOfWork.Instructor.Update(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            Instructor? instructorToBeDeleted = await _unitOfWork.Instructor.GetAsync(e => e.Id == id);
            if (instructorToBeDeleted == null)
                throw new EntityNotFoundException(id);

            _imageHandler.DeleteImageFile(instructorToBeDeleted.ImageUrl, _defaultImagePath);
            _unitOfWork.Instructor.Delete(instructorToBeDeleted);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
