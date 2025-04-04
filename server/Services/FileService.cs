using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class FileService
{
    private readonly string _webRootPath;
    private static readonly string[] ValidImageExtensions = { ".jpg", ".png", ".jpeg", ".gif" };

    public FileService(IWebHostEnvironment webHostEnvironment)
    {
        _webRootPath =
            webHostEnvironment.WebRootPath
            ?? throw new ArgumentNullException(nameof(webHostEnvironment));
    }

    /// <summary>
    /// Saves a list of image files to a specified folder and returns their URLs.
    /// </summary>
    /// <param name="files">List of image files to save.</param>
    /// <param name="folderName">Target folder name (default: "images").</param>
    /// <param name="entityId">Optional entity ID to include in filenames.</param>
    /// <param name="isUpdate">If true, overwrites existing files.</param>
    /// <returns>List of saved image URLs.</returns>
    /// <exception cref="ArgumentException">Thrown if no valid files are provided or file type is invalid.</exception>
    /// <exception cref="IOException">Thrown if file saving fails.</exception>
    public async Task<List<string>> SaveImagesAsync(
        List<IFormFile> files,
        string folderName = "images",
        int? entityId = null,
        bool isUpdate = false
    )
    {
        if (files == null || !files.Any(f => f != null && f.Length > 0))
            throw new ArgumentException("No valid files provided to save.");

        var imageUrls = new List<string>();
        var baseFolder = Path.Combine(_webRootPath, "images");
        var uploadFolder = Path.Combine(baseFolder, folderName);

        if (!Directory.Exists(uploadFolder))
            Directory.CreateDirectory(uploadFolder);

        var tasks = files.Select(
            async (file, i) =>
            {
                if (file == null || file.Length == 0)
                    return null;

                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!ValidImageExtensions.Contains(extension))
                    throw new ArgumentException($"File {file.FileName} is not a valid image.");

                string fileName = folderName switch
                {
                    "Products" when entityId.HasValue =>
                        $"Product-{entityId.Value}-{i + 1}{extension}",
                    "Users" when entityId.HasValue => $"User-{entityId.Value}{extension}",
                    _ => Guid.NewGuid().ToString() + extension,
                };

                var filePath = Path.Combine(uploadFolder, fileName);
                if (isUpdate && File.Exists(filePath))
                    File.Delete(filePath);

                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream).ConfigureAwait(false);
                    }
                    return $"/images/{folderName}/{fileName}";
                }
                catch (IOException ex)
                {
                    throw new IOException($"Failed to save file {fileName}: {ex.Message}", ex);
                }
            }
        );

        imageUrls = (await Task.WhenAll(tasks).ConfigureAwait(false))
            .Where(url => url != null)
            .Select(url => url!)
            .ToList();
        return imageUrls;
    }

    /// <summary>
    /// Saves image files temporarily to the "temp" folder and returns their URLs.
    /// </summary>
    /// <param name="files">List of image files to save temporarily.</param>
    /// <returns>List of temporary image URLs.</returns>
    /// <exception cref="ArgumentException">Thrown if no valid files are provided or file type is invalid.</exception>
    /// <exception cref="IOException">Thrown if file saving fails.</exception>
    public async Task<List<string>> SaveTempImagesAsync(List<IFormFile> files)
    {
        return await SaveImagesAsync(files, "temp", null, false).ConfigureAwait(false);
    }

    /// <summary>
    /// Moves images from the temp folder to a specified folder with new filenames.
    /// </summary>
    /// <param name="tempUrls">List of temporary image URLs.</param>
    /// <param name="folderName">Target folder name (e.g., "Products", "Users").</param>
    /// <param name="entityId">Entity ID to include in the filenames.</param>
    /// <returns>List of new image URLs.</returns>
    /// <exception cref="ArgumentNullException">Thrown if tempUrls is null.</exception>
    /// <exception cref="IOException">Thrown if file moving fails.</exception>
    public async Task<List<string>> MoveImagesToFolderAsync(
        List<string> tempUrls,
        string folderName,
        int entityId
    )
    {
        if (tempUrls == null)
            throw new ArgumentNullException(nameof(tempUrls));

        var imageUrls = new List<string>();
        var baseFolder = Path.Combine(_webRootPath, "images");
        var targetFolder = Path.Combine(baseFolder, folderName);

        if (!Directory.Exists(targetFolder))
            Directory.CreateDirectory(targetFolder);

        var tasks = tempUrls.Select(
            async (tempUrl, i) =>
            {
                var tempFilePath = Path.Combine(_webRootPath, tempUrl.TrimStart('/'));
                if (!File.Exists(tempFilePath))
                    return null;

                var extension = Path.GetExtension(tempUrl);
                string newFileName = folderName switch
                {
                    "Products" => $"Product-{entityId}-{i + 1}{extension}",
                    "Users" => $"User-{entityId}{extension}",
                    _ => Guid.NewGuid().ToString() + extension,
                };

                var newFilePath = Path.Combine(targetFolder, newFileName);
                try
                {
                    await Task.Run(() => File.Move(tempFilePath, newFilePath))
                        .ConfigureAwait(false);
                    return $"/images/{folderName}/{newFileName}";
                }
                catch (IOException ex)
                {
                    throw new IOException(
                        $"Failed to move file {tempUrl} to {newFileName}: {ex.Message}",
                        ex
                    );
                }
            }
        );

        imageUrls = (await Task.WhenAll(tasks).ConfigureAwait(false))
            .Where(url => url != null)
            .ToList();
        return imageUrls;
    }

    /// <summary>
    /// Deletes a list of images from the file system.
    /// </summary>
    /// <param name="imageUrls">List of image URLs to delete.</param>
    /// <exception cref="ArgumentNullException">Thrown if imageUrls is null.</exception>
    /// <exception cref="IOException">Thrown if file deletion fails.</exception>
    public async Task DeleteImagesAsync(List<string> imageUrls)
    {
        if (imageUrls == null)
            throw new ArgumentNullException(nameof(imageUrls));

        var tasks = imageUrls.Select(async url =>
        {
            var filePath = Path.Combine(_webRootPath, url.TrimStart('/'));
            if (File.Exists(filePath))
            {
                try
                {
                    await Task.Run(() => File.Delete(filePath)).ConfigureAwait(false);
                }
                catch (IOException ex)
                {
                    throw new IOException($"Failed to delete file {url}: {ex.Message}", ex);
                }
            }
        });

        await Task.WhenAll(tasks).ConfigureAwait(false);
    }

    /// <summary>
    /// Cleans up old temporary files in the "temp" folder based on age.
    /// </summary>
    /// <param name="maxAgeInMinutes">Maximum age of files to keep (in minutes, default: 60).</param>
    /// <exception cref="IOException">Thrown if file deletion fails.</exception>
    public async Task CleanTempFolderAsync(double maxAgeInMinutes = 60)
    {
        var tempFolder = Path.Combine(_webRootPath, "images", "temp");
        if (!Directory.Exists(tempFolder))
            return;

        var files = Directory.GetFiles(tempFolder);
        var now = DateTime.UtcNow;

        var tasks = files.Select(async file =>
        {
            var fileInfo = new FileInfo(file);
            if ((now - fileInfo.CreationTimeUtc).TotalMinutes > maxAgeInMinutes)
            {
                try
                {
                    await Task.Run(() => File.Delete(file)).ConfigureAwait(false);
                }
                catch (IOException ex)
                {
                    throw new IOException($"Failed to delete temp file {file}: {ex.Message}", ex);
                }
            }
        });

        await Task.WhenAll(tasks).ConfigureAwait(false);
    }
}
