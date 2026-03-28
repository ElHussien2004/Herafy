using Microsoft.AspNetCore.Http;
using ServiceAbstraction;
using Shared.CommonResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class FileService : IFileService
    {
        private readonly string[] _allowedExtensions = { ".jpg", ".png" };
        public async Task<Result<string>> SaveFileAsync(IFormFile file, string relativePath)
        {
            if (file == null || file.Length == 0)
                return Error.Validation("ملف غير صالح", "الملف فارغ");

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!_allowedExtensions.Contains(extension))
                return Error.Validation("نوع الملف غير مدعوم", "يرجى رفع صورة بصيغة jpg أو png");

            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var fullPath = Path.Combine(rootPath, relativePath);

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(fullPath, fileName);

            using var stream = new FileStream(filePath,FileMode.Create);
            await file.CopyToAsync(stream);

            var relative = Path.Combine(relativePath, fileName).Replace("\\", "/");

            return Result<string>.Ok(relative);
        }
        public Task<bool> DeleteFile(string filePath)
        {
            var fullPath = Path.Combine("wwwroot", filePath.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
