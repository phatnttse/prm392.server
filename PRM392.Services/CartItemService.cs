using AutoMapper;
using PRM392.Repositories.Entities;
using PRM392.Repositories.Interfaces;
using PRM392.Repositories.Models;
using PRM392.Services.DTOs.Cart;
using PRM392.Services.Interfaces;
using PRM392.Utils;


namespace PRM392.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CartItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApplicationResponse> AddCartItem(CreateUpdateCartItemDTO body)
        {
            try
            {
                if (body.Quantity <= 0) throw new ApiException("Quantity must be greater than 0", System.Net.HttpStatusCode.BadRequest);

                string currentUserId = Utilities.GetCurrentUserId() ?? throw new ApiException("User not found", System.Net.HttpStatusCode.NotFound);

                ApplicationUser? user = await _unitOfWork.UserAccountRepository.GetByIdAsync(currentUserId);

                Product? product = await _unitOfWork.ProductRepository.GetByIdAsync(body.ProductId!);

                if (product == null) throw new ApiException("Product not found", System.Net.HttpStatusCode.NotFound);

                CartItem? cartItem = await _unitOfWork.CartItemRepository.GetCartItemByUserIdAndProductId(currentUserId, body.ProductId!);

                int newQuantity = cartItem != null ? cartItem.Quantity + body.Quantity : body.Quantity;

                if (cartItem != null && newQuantity > cartItem!.Product!.StockQuantity) throw new ApiException("Product out of stock", System.Net.HttpStatusCode.BadRequest);

                if (cartItem != null)
                {
                    cartItem.Quantity = newQuantity;

                    _unitOfWork.CartItemRepository.Update(cartItem);
                }
                else
                {
                    cartItem = new CartItem
                    {
                        UserId = currentUserId,
                        ProductId = body.ProductId!,
                        Quantity = body.Quantity
                    };

                    await _unitOfWork.CartItemRepository.AddAsync(cartItem);
                }

                await _unitOfWork.SaveChangesAsync();

                return new ApplicationResponse
                {
                    Data = _mapper.Map<CartItemDTO>(cartItem),
                    Message = "Cart item added successfully",
                    Success = true,
                    StatusCode = System.Net.HttpStatusCode.Created
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

        public async Task<ApplicationResponse> ClearCartByUser()
        {
            try
            {
                string currentUserId = Utilities.GetCurrentUserId() ?? throw new ApiException("User not found", System.Net.HttpStatusCode.NotFound);

                ApplicationUser? user = await _unitOfWork.UserAccountRepository.GetByIdAsync(currentUserId);

                await _unitOfWork.CartItemRepository.ClearCartByUserId(currentUserId);

                await _unitOfWork.SaveChangesAsync();

                return new ApplicationResponse
                {
                    Message = "Cart cleared successfully",
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

        public async Task<ApplicationResponse> DeleteCartItem(string id)
        {
            try
            {
                CartItem cartItem = await _unitOfWork.CartItemRepository.GetByIdAsync(id) ?? throw new ApiException("Cart item not found", System.Net.HttpStatusCode.NotFound);

                _unitOfWork.CartItemRepository.Delete(cartItem);

                await _unitOfWork.SaveChangesAsync();

                return new ApplicationResponse
                {
                    Message = "Cart item deleted successfully",
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

        public async Task<ApplicationResponse> GetCartItemsByUser()
        {
            try
            {
                string currentUserId = Utilities.GetCurrentUserId() ?? throw new ApiException("User not found", System.Net.HttpStatusCode.NotFound);

                List<CartItem> cartItems = await _unitOfWork.CartItemRepository.GetCartItemsByUserId(currentUserId);

                return new ApplicationResponse
                {
                    Data = cartItems != null ? _mapper.Map<List<CartItemDTO>>(cartItems) : [],
                    Message = "Cart items retrieved successfully",
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

        public async Task<ApplicationResponse> UpdateCartItem(string id, CreateUpdateCartItemDTO body)
        {
            try
            {
                CartItem cartItem = await _unitOfWork.CartItemRepository.GetCartItemById(id) ?? throw new ApiException("Cart item not found", System.Net.HttpStatusCode.NotFound);

                int newQuantity = cartItem.Quantity + body.Quantity;

                if (newQuantity > cartItem.Product!.StockQuantity) throw new ApiException("Product out of stock", System.Net.HttpStatusCode.BadRequest);

                cartItem.Quantity = newQuantity;

                if (cartItem.Quantity <= 0)
                {
                    _unitOfWork.CartItemRepository.Delete(cartItem);
                }
                else
                {
                    _unitOfWork.CartItemRepository.Update(cartItem);
                }

                await _unitOfWork.SaveChangesAsync();

                return new ApplicationResponse
                {
                    Data = _mapper.Map<CartItemDTO>(cartItem),
                    Message = "Cart item updated successfully",
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
