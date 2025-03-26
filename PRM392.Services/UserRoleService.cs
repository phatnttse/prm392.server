using PRM392.Repositories.Entities;
using PRM392.Repositories.Models;
using PRM392.Services.Interfaces;
using Bonheur.Services.DTOs.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PRM392.Repositories.Interfaces;

namespace PRM392.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<UserRoleService> _logger;

        public UserRoleService(IUnitOfWork unitOfWork, SignInManager<ApplicationUser> signInManager, ILogger<UserRoleService> logger)
        {
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _logger = logger;
        }
        public async Task<ApplicationResponse> CreateRoleAsync(CreateUserRoleDTO body)
        {
            try
            {
                ApplicationRole role = new ApplicationRole
                {
                    Name = body.Name                
                };

                ApplicationRole? existingRole = await _unitOfWork.UserRoleRepository.GetRoleByNameAsync(role.Name!);

                if (existingRole != null) throw new ApiException("Role already exists", System.Net.HttpStatusCode.BadRequest);
                
                if (role.Id == null) role.Id = Guid.NewGuid().ToString();
                
                var result = await _unitOfWork.UserRoleRepository.CreateRoleAsync(role);

                if (!result.Succeeded) throw new ApiException(string.Join("; ", result.Errors.Select(error => error)), System.Net.HttpStatusCode.BadRequest);
                
                return new ApplicationResponse
                {
                    Message = $"Role {body.Name} created successfully",
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

        public async Task<ApplicationResponse> DeleteRoleAsync(string id)
        {
            try
            {
                var existingRole = await _unitOfWork.UserRoleRepository.GetRoleByIdAsync(id) ?? throw new ApiException("Role not found", System.Net.HttpStatusCode.NotFound);
                
                var result = await _unitOfWork.UserRoleRepository.DeleteRoleAsync(existingRole);

                if (!result.Succeeded) throw new ApiException(string.Join("; ", result.Errors.Select(error => error)), System.Net.HttpStatusCode.BadRequest);
                
                return new ApplicationResponse
                {
                    Message = $"Role {existingRole.Name} deleted successfully",
                    Success = true,
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = existingRole
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
