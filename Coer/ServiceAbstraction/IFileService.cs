using Microsoft.AspNetCore.Http;
using Shared.CommonResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IFileService
    {
        Task<Result<string>> SaveFileAsync(IFormFile file, string relativePath);
        Task<bool> DeleteFile(string filePath);
    }
}
