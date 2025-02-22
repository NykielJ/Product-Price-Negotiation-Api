using Microsoft.AspNetCore.Mvc;
using ProductPriceNegotiationApi.DTOs;
using ProductPriceNegotiationApi.Services;
using ProductPriceNegotiationApi.Utilities;
using ProductPriceNegotiationApi.Models;

namespace ProductPriceNegotiationApi.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public IActionResult AddProduct([FromBody] ProductDto productDto)
        {
            try
            {
                ValidationHelper.ValidateProductName(productDto.Name);
                ValidationHelper.ValidateProductPrice(productDto.Price);

                var product = new Product
                {
                    Name = productDto.Name,
                    Price = productDto.Price
                };

                _productService.AddProduct(product);
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = _productService.GetProduct(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAllProducts()
        {
            return Ok(_productService.GetAllProducts());
        }
    }
}
