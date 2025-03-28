using AutoMapper;
using PRM392.Repositories;
using PRM392.Repositories.Interfaces;
using PRM392.Repositories.Models;
using PRM392.Services.DTOs.Account;
using PRM392.Services.Interfaces;
using PRM392.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserAccountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApplicationResponse> GetProfile()
        {
            try
            {
                string currentUserId = Utilities.GetCurrentUserId() ?? throw new ApiException("Please ensure you are logged in.", System.Net.HttpStatusCode.Unauthorized);

                var userAndRole = await _unitOfWork.UserAccountRepository.GetUserAndRolesAsync(currentUserId);

                if (userAndRole == null) throw new ApiException("Please ensure you are logged in.", System.Net.HttpStatusCode.Unauthorized);

                var userData = _mapper.Map<UserAccountDTO>(userAndRole.Value.User);

                userData.Roles = userAndRole.Value.Roles;

                return new ApplicationResponse
                {
                    Success = true,
                    Message = "Account profile retrieved successfully.",
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = userData,
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
