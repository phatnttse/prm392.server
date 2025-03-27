using PRM392.Repositories.Models;
using PRM392.Services.DTOs.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services.Interfaces
{
    public interface ICartItemService
    {
        Task<ApplicationResponse> GetCartItemsByUser();
        Task<ApplicationResponse> AddCartItem(CreateUpdateCartItemDTO body);
        Task<ApplicationResponse> UpdateCartItem(string id, CreateUpdateCartItemDTO body);
        Task<ApplicationResponse> DeleteCartItem(string id);
        Task<ApplicationResponse> ClearCartByUser();
    }
}
