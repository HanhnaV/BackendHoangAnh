using BusinessObjects.Common;
using BusinessObjects.Products;
using DTOs.ProductDTOs.Request;
using DTOs.ProductDTOs.Response;
using Microsoft.AspNetCore.Mvc;
using Repositories.WorkSeeds.Interfaces;
using WebAPI.Middlewares;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResult<List<ProductResponse>>>> GetAll()
        {
            var repo = _unitOfWork.GetRepository<Product, Guid>();
            var products = await repo.GetAllAsync();
            var result = products.Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                SalePrice = p.SalePrice
            }).ToList();
            return Ok(ApiResult<List<ProductResponse>>.Success(result, "Products retrieved"));
        }

        [HttpPost("manager")]
        [ServiceFilter(typeof(ValidateModelAttribute))]
        public async Task<ActionResult<ApiResult<ProductResponse>>> Create([FromBody] CreateProductRequest request)
        {
            var repo = _unitOfWork.GetRepository<Product, Guid>();
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                SalePrice = request.SalePrice,
                CategoryId = request.CategoryId,
                Status = ProductStatus.Active
            };
            await repo.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            var response = new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                SalePrice = product.SalePrice
            };
            return Ok(ApiResult<ProductResponse>.Success(response, "Product created"));
        }

        [HttpDelete("manager/{id}")]
        public async Task<ActionResult<ApiResult<object>>> Delete(Guid id)
        {
            var repo = _unitOfWork.GetRepository<Product, Guid>();
            var success = await repo.SoftDeleteAsync(id);
            if (!success)
            {
                return NotFound(ApiResult<object>.Failure(new KeyNotFoundException("Product not found")));
            }
            await _unitOfWork.SaveChangesAsync();
            return Ok(ApiResult<object>.Success(null!, "Product deleted"));
        }
    }
}
