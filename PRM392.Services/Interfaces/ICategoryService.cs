using PRM392.Repositories.Models;
using PRM392.Services.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<ApplicationResponse> GetCategories();
        Task<ApplicationResponse> GetCategoryById(string id);
        Task<ApplicationResponse> CreateCategory(CreateUpdateCategoryDTO body);
        Task<ApplicationResponse> UpdateCategory(string id, CreateUpdateCategoryDTO body);
        Task<ApplicationResponse> DeleteCategory(string id);
    }
}
