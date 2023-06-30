using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_Api.Interfaces;

namespace dotnet_Api.Services
{
    public class UploadFileService : IUploadFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IConfiguration _config;

        public UploadFileService(IWebHostEnvironment webHostEnvironment, IConfiguration config) // access to wwwroot
        {
            _config = config;
            _webHostEnvironment = webHostEnvironment;

        }

        public bool IsUpload(List<IFormFile> formFiles) => formFiles != null && formFiles.Sum(f => f.Length) > 0;

        public string Validation(List<IFormFile> formFiles)
        {
            for (int ifIndex = 0; ifIndex < formFiles.Count; ifIndex++)
            {
                var formFile = formFiles[ifIndex];

                if (!ValidationExtension(formFile.FileName))
                {
                    return "Invalid file extension";
                }

                if (!ValidationSize(formFile.Length))
                {
                    return "The file is too large";
                }
            }

            return null;
        }

        public async Task<List<string>> UploadImage(List<IFormFile> formFiles)
        {
            List<string> listFileName = new List<string>();

            string uploadPath = $"{_webHostEnvironment.WebRootPath}/images/";

            foreach (var formFile in formFiles)
            {
                string filelName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName); // generate encoded file name
                string fullPath = uploadPath + filelName;

                using (var stream = File.Create(fullPath))
                {
                    await formFile.CopyToAsync(stream);
                }

                listFileName.Add(filelName);
            }

            return listFileName;
        }


        public bool ValidationExtension(string fileName)
        {
            string[] permittedExtensions = { ".jpg", ".png" };

            var ext = Path.GetExtension(fileName).ToLower();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext)) return false;

            return true;
        }

        public bool ValidationSize(long fileSize) => _config.GetValue<long>("FileSizeLimit") > fileSize;
    }
}