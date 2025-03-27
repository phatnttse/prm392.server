using Microsoft.AspNetCore.Mvc;
using PRM392.Services.DTOs.Product;
using PRM392.Services.Interfaces;

namespace PRM392.API.Controllers
{
    /// <summary>
    /// Controller for managing products.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/products")]
    [ApiVersion("1.0")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="productService">The product service.</param>
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>A list of products.</returns>
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return Ok(await _productService.GetProducts());
        }

        /// <summary>
        /// Gets a product by id.
        /// </summary>
        /// <param name="id">The product id.</param>
        /// <returns>The product with the specified id.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            return Ok(await _productService.GetProductById(id));
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="body">The product details.</param>
        /// <returns>The created product.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateUpdateProductDTO body)
        {
            return Created("", await _productService.CreateProduct(body));
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The product id.</param>
        /// <param name="body">The updated product details.</param>
        /// <returns>The updated product.</returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] CreateUpdateProductDTO body)
        {
            return Ok(await _productService.UpdateProduct(id, body));
        }

        /// <summary>
        /// Deletes a product.
        /// </summary>
        /// <param name="id">The product id.</param>
        /// <returns>A confirmation of the deletion.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            return Ok(await _productService.DeleteProduct(id));
        }
    }
}
