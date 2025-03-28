using PRM392.Repositories.Models;
using PRM392.Services.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services.Interfaces
{
    public interface IProductService
    {
        Task<ApplicationResponse> GetProducts();
        Task<ApplicationResponse> GetProductById(string shopId);
        Task<ApplicationResponse> CreateProduct(CreateUpdateProductDTO body);
        Task<ApplicationResponse> UpdateProduct(string id, CreateUpdateProductDTO body);
        Task<ApplicationResponse> DeleteProduct(string id);
        Task<ApplicationResponse> GetListProductAfterFilterByPrice(decimal minPrice, decimal maxPrice);
        Task<ApplicationResponse> GetListProductAfterFilterByCategory(string categoryId);
    }
}
