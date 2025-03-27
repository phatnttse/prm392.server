using PRM392.Repositories.Models;
using PRM392.Services.DTOs.Cart;
using PRM392.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemService _cartItemService;

        public CartItemService(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        public Task<ApplicationResponse> AddCartItem(CreateUpdateCartItemDTO request)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationResponse> ClearCartByUser()
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationResponse> DeleteCartItem(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationResponse> GetCartItemsByUser()
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationResponse> UpdateCartItem(string id, CreateUpdateCartItemDTO request)
        {
            throw new NotImplementedException();
        }
    }
}
