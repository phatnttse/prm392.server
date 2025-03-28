using AutoMapper;
using Microsoft.Extensions.Logging;
using Net.payOS.Types;
using Net.payOS;
using PRM392.Repositories.Entities;
using PRM392.Repositories.Enums;
using PRM392.Repositories.Interfaces;
using PRM392.Repositories.Models;
using PRM392.Services.DTOs.Payment;
using PRM392.Services.Interfaces;


namespace PRM392.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PayOS _payOS;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper, PayOS payOS, ILogger<PaymentService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _payOS = payOS;
            _logger = logger;
        }

        public async Task<ApplicationResponse> GetPaymentRequestInfo(int orderCode)
        {
            try
            {
                PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInformation(orderCode);

                if (paymentLinkInformation == null) throw new ApiException("Payment information not found", System.Net.HttpStatusCode.NotFound);

                return new ApplicationResponse
                {
                    Data = paymentLinkInformation,
                    Message = "Payment information retrieved successfully",
                    Success = true,
                    StatusCode = System.Net.HttpStatusCode.OK
                };

            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<PaymentResponse> PayOsTransferHandler(WebhookType body)
        {
            try
            {

                WebhookData data = _payOS.verifyPaymentWebhookData(body);

                if (data.description == "Ma giao dich thu nghiem" || data.description == "VQRIO123") return new PaymentResponse(0, "Ok", null); // confirm webhook

                if (data.code == "00")
                {
                    Order? order = await _unitOfWork.OrderRepository.GetOrderByCodeAsync((int)data.orderCode);

                    if (order == null) throw new ApiException("Order not found", System.Net.HttpStatusCode.NotFound);

                    List<Product> products = order.OrderDetails!.Select(x => x.Product).ToList()!;

                    if (products != null && products.Count > 0)
                    {
                        foreach (Product product in products)
                        {
                            product.StockQuantity -= order.OrderDetails!.FirstOrDefault(x => x.ProductId == product.Id)?.Quantity ?? 0;

                            _unitOfWork.ProductRepository.Update(product);
                        }
                    }
                 
                    order.PaymentStatus = PaymentStatus.Paid;

                    _unitOfWork.OrderRepository.Update(order);

                    await _unitOfWork.CartItemRepository.ClearCartByUserId(order.UserId!);

                    await _unitOfWork.SaveChangesAsync();

                    return new PaymentResponse(0, "Ok", null);

                }

                return new PaymentResponse(-1, "Fail", null);

            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }

}
