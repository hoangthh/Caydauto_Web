using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class FileService
{
    private readonly string _webRootPath;

    public FileService(IWebHostEnvironment webHostEnvironment)
    {
        _webRootPath = webHostEnvironment.WebRootPath;
    }

    /// <summary>
    /// Lưu danh sách các tệp hình ảnh vào thư mục cụ thể và trả về danh sách URL
    /// </summary>
    public async Task<List<string>> SaveImagesAsync(
        List<IFormFile> files,
        string folderName = "images",
        int? entityId = null,
        bool isUpdate = false
    )
    {
        if (files == null || !files.Any(f => f != null))
            throw new ArgumentException("No valid files provided to save.");

        var imageUrls = new List<string>();
        var baseFolder = Path.Combine(_webRootPath, "images");
        var uploadFolder = Path.Combine(baseFolder, folderName);

        if (!Directory.Exists(uploadFolder))
            Directory.CreateDirectory(uploadFolder);

        for (int i = 0; i < files.Count; i++)
        {
            var file = files[i];
            if (file != null && file.Length > 0)
            {
                string fileName;
                if (folderName == "Products" && entityId.HasValue)
                {
                    fileName =
                        $"Product-{entityId.Value}-{i + 1}{Path.GetExtension(file.FileName)}";
                }
                else if (folderName == "Users" && entityId.HasValue)
                {
                    fileName = $"User-{entityId.Value}{Path.GetExtension(file.FileName)}";
                }
                else
                {
                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                }

                var filePath = Path.Combine(uploadFolder, fileName);

                if (isUpdate && File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream).ConfigureAwait(false);
                }

                imageUrls.Add($"/images/{folderName}/{fileName}");
            }
        }

        return imageUrls;
    }

    /// <summary>
    /// Lưu hình ảnh tạm thời vào thư mục temp và trả về danh sách URL tạm
    /// </summary>
    public async Task<List<string>> SaveTempImagesAsync(List<IFormFile> files)
    {
        return await SaveImagesAsync(files, "temp", null, false).ConfigureAwait(false);
    }

    /// <summary>
    /// Di chuyển hình ảnh từ thư mục temp sang thư mục đích với tên file mới
    /// </summary>
    public List<string> MoveImagesToFolderAsync(
        List<string> tempUrls,
        string folderName,
        int entityId
    )
    {
        var imageUrls = new List<string>();
        var baseFolder = Path.Combine(_webRootPath, "images");
        var targetFolder = Path.Combine(baseFolder, folderName);

        if (!Directory.Exists(targetFolder))
            Directory.CreateDirectory(targetFolder);

        for (int i = 0; i < tempUrls.Count; i++)
        {
            var tempUrl = tempUrls[i];
            var tempFilePath = Path.Combine(_webRootPath, tempUrl.TrimStart('/'));
            if (!File.Exists(tempFilePath))
                continue;

            var extension = Path.GetExtension(tempUrl);
            string newFileName;

            if (folderName == "Products")
            {
                newFileName = $"Product-{entityId}-{i + 1}{extension}";
            }
            else if (folderName == "Users")
            {
                newFileName = $"User-{entityId}{extension}";
            }
            else
            {
                newFileName = Guid.NewGuid().ToString() + extension;
            }

            var newFilePath = Path.Combine(targetFolder, newFileName);
            File.Move(tempFilePath, newFilePath);

            imageUrls.Add($"/images/{folderName}/{newFileName}");
        }

        return imageUrls;
    }

    /// <summary>
    /// Xóa danh sách hình ảnh
    /// </summary>
    public void DeleteImagesAsync(List<string> imageUrls)
    {
        foreach (var url in imageUrls)
        {
            var filePath = Path.Combine(_webRootPath, url.TrimStart('/'));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
