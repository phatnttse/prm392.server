using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRM392.Services.DTOs.Order;
using PRM392.Services.Interfaces;

namespace PRM392.API.Controllers
{
    /// <summary>
    /// Controller for handling order-related operations.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/orders")]
    [ApiVersion("1.0")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderController"/> class.
        /// </summary>
        /// <param name="orderService">The order service.</param>
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Gets the order by code.
        /// </summary>
        /// <param name="orderCode">The order code.</param>
        /// <returns>The order details.</returns>
        [HttpGet("{orderCode}")]
        [Authorize]
        public async Task<IActionResult> GetOrderByCode(int orderCode)
        {
            return Ok(await _orderService.GetOrderByCode(orderCode));
        }

        /// <summary>
        /// Creates a new order. PaymentMethod (1: Cash, 2: BankTransfer).
        /// </summary>
        /// <param name="createOrderDTO">The order details.</param>
        /// <returns>The result of the creation operation.</returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO createOrderDTO)
        {
            return Ok(await _orderService.CreateOrder(createOrderDTO));
        }

        /// <summary>
        /// Gets the order history for the current user.
        /// </summary>
        /// <returns>The list of orders for the user.</returns>
        [HttpGet("history")]
        [Authorize]
        public async Task<IActionResult> GetOrdersByUser()
        {
            return Ok(await _orderService.GetOrdersByUser());
        }
    }
}
