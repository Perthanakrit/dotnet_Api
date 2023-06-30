using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_Api.Interfaces
{
    public interface IUploadFileService
    {
        bool IsUpload(List<IFormFile> formFiles);

        string Validation(List<IFormFile> formFiles);
        Task<List<string>> UploadImage(List<IFormFile> formFiles);

    }
}