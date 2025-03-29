using AutoMapper;
using Net.payOS;
using Net.payOS.Types;
using PRM392.Repositories.Entities;
using PRM392.Repositories.Enums;
using PRM392.Repositories.Interfaces;
using PRM392.Repositories.Models;
using PRM392.Services.DTOs.Order;
using PRM392.Services.Interfaces;
using PRM392.Utils;


namespace PRM392.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly string _payOsPaymentReturnUrl;
        private readonly PayOS _payOS;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, PayOS payOS)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _payOS = payOS;
            _payOsPaymentReturnUrl = Environment.GetEnvironmentVariable("PAYOS_PAYMENT_RETURN_URL") ?? throw new ApiException("PAYOS_PAYMENT_RETURN_URL is not set", System.Net.HttpStatusCode.InternalServerError);
        }

        public async Task<ApplicationResponse> CreateOrder(CreateOrderDTO body)
        {
            try
            {
                string currentUserId = Utilities.GetCurrentUserId() ?? throw new ApiException("Please ensure you are logged in.", System.Net.HttpStatusCode.Unauthorized);

                ApplicationUser user = await _unitOfWork.UserAccountRepository.GetByIdAsync(currentUserId) ?? throw new ApiException("User does not exist", System.Net.HttpStatusCode.NotFound);


                Order order = _mapper.Map<Order>(body);
                order.UserId = currentUserId;
                order.OrderCode = int.Parse(DateTimeOffset.UtcNow.ToString("ffffff"));
                order.Status = OrderStatus.Pending;
                order.PaymentStatus = PaymentStatus.Pending;

                await _unitOfWork.OrderRepository.AddAsync(order);

                List<CartItem> cartItems = await _unitOfWork.CartItemRepository.GetCartItemsByUserId(currentUserId);

                if (cartItems == null || cartItems.Count == 0) throw new ApiException("Cart is empty", System.Net.HttpStatusCode.BadRequest);

                List<OrderDetail> orderDetails = cartItems.Select(item =>
                new OrderDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.ProductId != null ? item!.Product!.Price : 0,
                    OrderId = order.Id
                }).ToList();

                List<OrderDetail> newOrderDetails = await _unitOfWork.OrderDetailRepository.AddRangeAsync(orderDetails);

                order.OrderDetails = newOrderDetails;

                decimal amount = newOrderDetails.Sum(x => x.Price * x.Quantity);

                order.Amount = amount;

                if (body.PaymentMethod == PaymentMethod.Cash)
                {
                    await _unitOfWork.CartItemRepository.ClearCartByUserId(currentUserId);

                    await _unitOfWork.SaveChangesAsync();

                    return new ApplicationResponse
                    {
                        Data = _mapper.Map<OrderDTO>(order),
                        Message = "Order with cash method successfully",
                        Success = true,
                        StatusCode = System.Net.HttpStatusCode.Created
                    };
                }
                else if (body.PaymentMethod == PaymentMethod.BankTransfer)
                {
                    await _unitOfWork.SaveChangesAsync();

                    List<ItemData> items = newOrderDetails.Select(od =>
                    new ItemData(od.Product?.Name ?? "Unknown", od.Quantity, (int)od.Price)).ToList();

                    PaymentData paymentData = new PaymentData(order.OrderCode, (int)amount, "Flower Shop Checkout", items, this._payOsPaymentReturnUrl, this._payOsPaymentReturnUrl);

                    CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

                    return new ApplicationResponse
                    {
                        Data = createPayment,
                        Message = "Order with bank transfer payment method successfully",
                        Success = true,
                        StatusCode = System.Net.HttpStatusCode.Created
                    };
                }
                else
                {
                    return new ApplicationResponse
                    {
                        Message = "Payment method not supported",
                        Success = false,
                        StatusCode = System.Net.HttpStatusCode.BadRequest
                    };
                }
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

        public async Task<ApplicationResponse> GetOrderByCode(int orderCode)
        {
            try
            {
                Order order = await _unitOfWork.OrderRepository.GetOrderByCodeAsync(orderCode) ?? throw new ApiException("Order not found", System.Net.HttpStatusCode.NotFound);

                return new ApplicationResponse
                {
                    Data = _mapper.Map<OrderDTO>(order),
                    Message = "Order retrieved successfully",
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

        public async Task<ApplicationResponse> GetOrdersByUser()
        {
            try
            {
                string currentUserId = Utilities.GetCurrentUserId() ?? throw new ApiException("Please ensure you are logged in.", System.Net.HttpStatusCode.Unauthorized);

                List<Order> orders = await _unitOfWork.OrderRepository.GetOrdersByUserIdAsync(currentUserId);

                return new ApplicationResponse
                {
                    Data = _mapper.Map<List<OrderDTO>>(orders),
                    Message = "Orders retrieved successfully",
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
    }
}
