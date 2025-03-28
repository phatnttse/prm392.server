using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Net.payOS;
using PRM392.Services.Interfaces;

namespace PRM392.API.Controllers
{
    /// <summary>
    /// Controller for handling payment-related operations.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/payment")]
    [ApiVersion("1.0")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly PayOS _payOS;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentController"/> class.
        /// </summary>
        /// <param name="paymentService">The payment service.</param>
        /// <param name="payOS">The PayOS instance.</param>
        public PaymentController(IPaymentService paymentService, PayOS payOS)
        {
            _paymentService = paymentService;
            _payOS = payOS;
        }

        /// <summary>
        /// Webhook handler for PayOS transfer.
        /// </summary>
        /// <param name="body">The webhook body.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpPost("payos_transfer_handler")]
        public async Task<IActionResult> PayOsTransferHandler(WebhookType body)
        {
            return Ok(await _paymentService.PayOsTransferHandler(body));
        }

        /// <summary>
        /// Gets the payment request information.
        /// </summary>
        /// <param name="orderCode">The order code.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpGet("info/{orderCode}")]
        [Authorize]
        public async Task<IActionResult> GetPaymentRequestInfo(int orderCode)
        {
            return Ok(await _paymentService.GetPaymentRequestInfo(orderCode));
        }
    }
}
