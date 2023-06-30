using Microsoft.AspNetCore.Mvc;
using dotnet_Api.Entities;
using System.Net;
using dotnet_Api.DTOs.Product;
using Mapster;
using dotnet_Api.Interfaces;

namespace dotnet_Api.Controllers;

[ApiController]
[Route("[controller]")] //... localhost: 5001/weatherforecast [] -> data unknowtation
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService) => _productService = productService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProducts() => (await _productService.FindAll()) // มี () thus ต้องการให้ await ทำงานเสร็จก่นอใน ()
                .Select(ProductResponse.FromProduct).ToList(); //Enitity to DTOs

    [HttpGet("{id}")] //localhost: 5001/roductsforecast/{id}(int)
    public async Task<ActionResult<ProductResponse>> GetProductsById(int id)
    {
        var product = await _productService.FindById(id);
        if (product == null)
        {
            return NotFound();
        }

        return product.Adapt<ProductResponse>();
    }


    [HttpGet("search")] //localhost: 5001/roductsforecast/search?name=imac
    public async Task<ActionResult<IEnumerable<ProductResponse>>> SearchProduct([FromQuery] string name = "")
    {
        var result = (await _productService.Search(name)) //Name in Product is contained in name
                        .Select(ProductResponse.FromProduct) // > Eniity to DTOs
                        .ToList();

        return result;
    }

    [HttpPost] //localhost: 5001/roductsforecast (body JSON)
    public async Task<ActionResult<Product>> AddProduct([FromForm] ProductRequest productRequest) //[FromForm] -> multi-path [FormBody] (JSON)
    {
        (string errorMessage, string imageName) = await _productService.UploadImage(productRequest.FormFiles);
        if (!String.IsNullOrEmpty(errorMessage)) return BadRequest();

        var product = productRequest.Adapt<Product>();

        product.Image = imageName; //upload image

        await _productService.Create(product);

        return StatusCode((int)HttpStatusCode.Created);
        // unit of work **
    }

    [HttpPut("{id}")] //localhost: 5001/roducts/123
    public async Task<ActionResult<Product>> UpdateProduct(int id, [FromForm] ProductRequest productRequest) //[FromForm] -> multi-path [FormBody] ()
    {
        if (id != productRequest.ProductId) return BadRequest();

        var product = await _productService.FindById(id);

        if (product == null) return NotFound();

        //uploading Image
        (string errorMessage, string imageName) = await _productService.UploadImage(productRequest.FormFiles);
        if (!String.IsNullOrEmpty(errorMessage)) return BadRequest();

        if (!String.IsNullOrEmpty(imageName))
        {
            product.Image = imageName;
        }

        //mapping using maspter
        productRequest.Adapt(product);

        await _productService.Upadte(product);

        return NoContent();
    }

    [HttpDelete("{id}")] //localhost: 5001/roducts/123
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await _productService.FindById(id);

        if (product == null) return NotFound();

        await _productService.Delete(product);
        return NoContent();
    }
}

// มี error หลังทำ IService หรือ depancany injection becasuse ยังไม่ app service ใน program.cs
