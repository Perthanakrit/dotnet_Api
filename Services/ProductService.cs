using dotnet_Api.Data;
using dotnet_Api.Entities;
using dotnet_Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace dotnet_Api.Services
{
    public class ProductService : IProductService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IUploadFileService _uploadFileService;

        public ProductService(DatabaseContext databaseContext, IUploadFileService uploadFileService)
        {
            _uploadFileService = uploadFileService;
            _databaseContext = databaseContext;
        }

        public async Task<IEnumerable<Product>> FindAll()
        {
            return await _databaseContext.Products.Include(p => p.Category)
                        .OrderByDescending(p => p.ProductId) // -> Entity
                        .ToListAsync();
        }
        public async Task<Product> FindById(int id)
        {
            return await _databaseContext.Products.Include(p => p.Category)
                        .SingleOrDefaultAsync(p => p.ProductId == id); //SingleOrDefault
        }

        public async Task<IEnumerable<Product>> Search(string name)
        {
            return await _databaseContext.Products.Include(p => p.Category)
                        .Where(p => p.Name.ToLower().Contains(name.ToLower())) //Name in Product is contained in name
                        .ToListAsync();
        }
        public async Task Create(Product product)
        {
            _databaseContext.Products.Add(product);
            await _databaseContext.SaveChangesAsync(); // save

        }

        public async Task Upadte(Product product)
        {
            _databaseContext.Products.Update(product);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task Delete(Product product)
        {
            _databaseContext.Products.Remove(product);
            await _databaseContext.SaveChangesAsync();

        }

        public async Task<(string errorMessage, string imageName)> UploadImage(List<IFormFile> formFiles)
        {
            String errorMessage = String.Empty;
            String imageName = String.Empty;

            if (_uploadFileService.IsUpload(formFiles))
            {
                errorMessage = _uploadFileService.Validation(formFiles);
                if (String.IsNullOrEmpty(errorMessage))
                {
                    imageName = (await _uploadFileService.UploadImage(formFiles))[0];
                }
            }

            return (errorMessage, imageName);
        }
    }
}