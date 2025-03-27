using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRM392.Repositories.Models;
using PRM392.Services.DTOs.Category;
using PRM392.Services.Interfaces;

namespace PRM392.API.Controllers
{
    /// <summary>
    /// Controller for managing categories.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryController"/> class.
        /// </summary>
        /// <param name="categoryService">The category service.</param>
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Gets the list of categories.
        /// </summary>
        /// <returns>A list of categories.</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ApplicationResponse))]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(await _categoryService.GetCategories());
        }

        /// <summary>
        /// Gets a category by its identifier.
        /// </summary>
        /// <param name="id">The category identifier.</param>
        /// <returns>The category with the specified identifier.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ApplicationResponse))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            return Ok(await _categoryService.GetCategoryById(id));
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="body">The category data transfer object.</param>
        /// <returns>The created category.</returns>
        [HttpPost()]
        [ProducesResponseType(201, Type = typeof(ApplicationResponse))]
        //[Authorize(Roles = Constants.Roles.ADMIN)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateUpdateCategoryDTO body)
        {
            return Created("", await _categoryService.CreateCategory(body));
        }

        /// <summary>
        /// Updates a category.
        /// </summary>
        /// <param name="id">The category identifier.</param>
        /// <param name="body">The category data transfer object.</param>
        /// <returns>The updated category.</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(200, Type = typeof(ApplicationResponse))]
        [ProducesResponseType(404)]
        //[Authorize(Roles = Constants.Roles.ADMIN)]
        public async Task<IActionResult> UpdateCategoryProfile(string id, [FromBody] CreateUpdateCategoryDTO body)
        {
            return Ok(await _categoryService.UpdateCategory(id, body));
        }

        /// <summary>
        /// Deletes a category.
        /// </summary>
        /// <param name="id">The category identifier.</param>
        /// <returns>The result of the delete operation.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(ApplicationResponse))]
        [ProducesResponseType(404)]
        //[Authorize(Roles = Constants.Roles.ADMIN)]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            return Ok(await _categoryService.DeleteCategory(id));
        }
    }
}
