using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Utility
{
    public class ImageHandler
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageHandler(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Creates a unique name for the given file and returns its path relative to wwwroot.
        /// If the file is null, returns the relative path for the default image.
        /// </summary>
        /// <param name="file">The image file.</param>
        /// <param name="imagesDir">The path to the images directory relative to wwwroot.</param>
        /// <param name="defaultImageFileName">The file name of the default image.</param>
        /// <returns>The path to the new or default image relative to wwwroot</returns>
        public string GetNewOrDefaultImagePath(IFormFile? file, string imagesDir, string defaultImageFileName)
        {
            if (string.IsNullOrEmpty(imagesDir))
                throw new ArgumentException("Images directory cannot be null or empty.", nameof(imagesDir));

            if (string.IsNullOrEmpty(defaultImageFileName))
                throw new ArgumentException("Default image file name cannot be null or empty.", nameof(defaultImageFileName));

            string imageFileName = file != null
                ? Path.GetRandomFileName() + Path.GetExtension(file.FileName)
                : defaultImageFileName;

            return Path.Combine("\\", FormatPath(imagesDir), FormatPath(imageFileName));
        }

        /// <summary>
        /// Deletes the image file located at the specified imagePath, excluding the default image.
        /// </summary>
        /// <param name="imagePath">The path to the image file relative to wwwroot.</param>
        /// <param name="defaultImagePath">The path to the default image file relative to wwwroot (optional).</param>
        public void DeleteImageFile(string imagePath, string defaultImagePath = "")
        {
            try
            {
                if (imagePath != defaultImagePath)
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, FormatPath(imagePath));
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting image file: {ex.Message}");
            }
        }

        /// <summary>
        /// Saves the provided image file to the specified imagePath.
        /// </summary>
        /// <param name="file">The image file to save.</param>
        /// <param name="imagePath">The path where the image will be saved relative to wwwroot.</param>
        /// <exception cref="ArgumentException">Thrown if the imagePath is null or empty.</exception>
        public void SaveImage(IFormFile file, string imagePath)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath))
                    throw new ArgumentException("The imagePath cannot be null or empty.", nameof(imagePath));

                string fullDirPath = Path.Combine(_webHostEnvironment.WebRootPath, FormatPath(imagePath));

                using var fileStream = new FileStream(fullDirPath, FileMode.Create);
                file.CopyTo(fileStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving image file: {ex.Message}");
                throw;
            }
        }

        private string FormatPath(string path)
        {
            return path.TrimStart('~', '/').Replace("/", Path.DirectorySeparatorChar.ToString()).TrimStart('\\');
        }
    }
}
