using PRM392.Repositories.Models;
using PRM392.Services.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ApplicationResponse> CreateOrder(CreateOrderDTO body);
        Task<ApplicationResponse> GetOrderByCode(int orderCode);
        Task<ApplicationResponse> GetOrdersByUser();
    }
}
