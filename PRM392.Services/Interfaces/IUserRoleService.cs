using Bonheur.Services.DTOs.Account;
using PRM392.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services.Interfaces
{
    public interface IUserRoleService
    {
        Task<ApplicationResponse> CreateRoleAsync(CreateUserRoleDTO body);
        Task<ApplicationResponse> DeleteRoleAsync(string id);
    }
}
