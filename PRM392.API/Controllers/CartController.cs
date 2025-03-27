using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRM392.Services.DTOs.Cart;
using PRM392.Services.Interfaces;

namespace PRM392.API.Controllers
{
    /// <summary>
    /// Controller for managing cart items.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartController"/> class.
        /// </summary>
        /// <param name="cartItemService">The cart item service.</param>
        public CartController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        /// <summary>
        /// Gets the cart items for the authenticated user.
        /// </summary>
        /// <returns>A list of cart items.</returns>
        [HttpGet("items")]
        [Authorize]
        public async Task<IActionResult> GetCartItems()
        {
            return Ok( await _cartItemService.GetCartItemsByUser());
        }

        /// <summary>
        /// Adds a new item to the cart.
        /// </summary>
        /// <param name="body">The cart item to add.</param>
        /// <returns>The result of the add operation.</returns>
        [HttpPost("items")]
        [Authorize]
        public async Task<IActionResult> AddCartItem(CreateUpdateCartItemDTO body)
        {
            return Ok(await _cartItemService.AddCartItem(body));
        }

        /// <summary>
        /// Updates an existing cart item.
        /// </summary>
        /// <param name="id">The ID of the cart item to update.</param>
        /// <param name="body">The updated cart item details.</param>
        /// <returns>The result of the update operation.</returns>
        [HttpPatch("items/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCartItem(string id, CreateUpdateCartItemDTO body)
        {
            return Ok(await _cartItemService.UpdateCartItem(id, body));
        }

        /// <summary>
        /// Deletes a cart item.
        /// </summary>
        /// <param name="id">The ID of the cart item to delete.</param>
        /// <returns>The result of the delete operation.</returns>
        [HttpDelete("items/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCartItem(string id)
        {
            return Ok(await _cartItemService.DeleteCartItem(id));
        }

        /// <summary>
        /// Deletes all cart items for the authenticated user.
        /// </summary>
        /// <returns>The result of the delete operation.</returns>
        [HttpDelete("items")]
        [Authorize]
        public async Task<IActionResult> DeleteAllCartItems()
        {
            return Ok(await _cartItemService.ClearCartByUser());
        }
    }
}
