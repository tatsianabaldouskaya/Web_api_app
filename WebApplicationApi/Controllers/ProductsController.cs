using Microsoft.AspNetCore.Mvc;

using WebApplicationApi.Authentication;
using WebApplicationApi.Models.DataModels;
using WebApplicationApi.Models.Dtos.Product;
using WebApplicationApi.Repositories.Interfaces;

namespace WebApplicationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public class ProductsController : ControllerBase
    {
        private readonly IRepository<ProductModel> _repository;

        public ProductsController(IRepository<ProductModel> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetProducts()
        {
            var products = await _repository.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetProduct(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(new { message = "Product was not found" });
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductModel>> CreateProduct(ProductDto productDto)
        {
            var newProduct = new ProductModel()
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Author = productDto.Author,
                Price = productDto.Price,
                ImagePath = productDto.ImagePath,
            };

            var createdProduct = await _repository.AddAsync(newProduct);

            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDto productDto)
        {
            var updatedProduct = new ProductModel
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Author = productDto.Author,
                Price = productDto.Price,
                ImagePath = productDto.ImagePath
            };

            var result = await _repository.UpdateAsync(id, updatedProduct);

            if (result == null)
            {
                return NotFound(new { message = "Product was not found" });
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
       // [Authorize(Roles = nameof(Role.Manager))]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleted = await _repository.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new { message = "Product was not found" });
            }
            Console.WriteLine("Product was deleted");
            return Ok();
        }
    }
}
