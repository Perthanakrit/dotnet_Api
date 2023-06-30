using dotnet_Api.Entities;

namespace dotnet_Api.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> FindAll(); // fucntion define
        Task<Product> FindById(int id);
        Task Create(Product product);
        Task Upadte(Product product);
        Task Delete(Product product);
        Task<IEnumerable<Product>> Search(string name);
        Task<(string errorMessage, string imageName)> UploadImage(List<IFormFile> formFiles);
    }
}