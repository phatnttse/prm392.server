using Bonheur.Services.DTOs.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRM392.Repositories.Models;
using PRM392.Services.Interfaces;

namespace PRM392.API.Controllers
{
    /// <summary>
    /// Controller for managing user roles.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/roles")]
    [ApiVersion("1.0")]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRoleController"/> class.
        /// </summary>
        /// <param name="userRoleService">The user role service.</param>
        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        /// <summary>
        /// Creates a new role.
        /// </summary>
        /// <param name="body">The role details.</param>
        /// <returns>The result of the role creation.</returns>
        [HttpPost("roles")]
        [ProducesResponseType(200, Type = typeof(ApplicationResponse))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateRole([FromBody] CreateUserRoleDTO body)
        {
            return Ok(await _userRoleService.CreateRoleAsync(body));
        }

        /// <summary>
        /// Deletes a role by ID.
        /// </summary>
        /// <param name="id">The role ID.</param>
        /// <returns>The result of the role deletion.</returns>
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
