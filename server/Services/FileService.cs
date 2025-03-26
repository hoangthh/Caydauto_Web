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
    /// <param name="files">Danh sách các tệp hình ảnh (IFormFile)</param>
    /// <param name="folderName">Tên thư mục (Products, Users, ...), nếu null thì mặc định là "images"</param>
    /// <param name="entityId">ID của thực thể (ProductId, UserId, ...)</param>
    /// <param name="isUpdate">Nếu true, ghi đè ảnh cũ</param>
    /// <returns>Danh sách URL của các tệp đã lưu</returns>
    /// <exception cref="ArgumentException">Ném ra nếu danh sách files rỗng hoặc null</exception>
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

        // Đảm bảo thư mục tồn tại
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
                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); // Mặc định
                }

                var filePath = Path.Combine(uploadFolder, fileName);

                // Nếu là cập nhật và file cũ tồn tại, ghi đè
                if (isUpdate && File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // Lưu tệp
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Thêm URL vào danh sách
                imageUrls.Add($"/images/{folderName}/{fileName}");
            }
        }

        return imageUrls;
    }
}
