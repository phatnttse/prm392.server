using Bonheur.Services.DTOs.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRM392.Repositories.Models;
using PRM392.Services.Interfaces;

namespace PRM392.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/roles")]
    [ApiVersion("1.0")]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        /// <summary>
        /// Tạo role
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("roles")]
        [ProducesResponseType(200, Type = typeof(ApplicationResponse))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateRole([FromBody] CreateUserRoleDTO body)
        {
            return Ok(await _userRoleService.CreateRoleAsync(body));
        }

        /// <summary>
        /// Xoá role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("roles/{id}")]
        [ProducesResponseType(200, Type = typeof(ApplicationResponse))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteRole(string id)
        {
            return Ok(await _userRoleService.DeleteRoleAsync(id));
        }
    }
}
