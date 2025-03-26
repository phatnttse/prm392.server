using PRM392.Repositories.Entities;
using PRM392.Repositories.Models;
using PRM392.Services.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApplicationUser> HandleLoginAsync(string username, string password);
        Task<ClaimsPrincipal> CreateClaimsPrincipalAsync(ApplicationUser user, IEnumerable<string> scopes);
        Task<bool> CanSignInAsync(ApplicationUser user);
        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task<ApplicationResponse> SignUp(CreateAccountDTO body);
    }
}
