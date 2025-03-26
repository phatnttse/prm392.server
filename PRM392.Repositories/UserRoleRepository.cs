using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PRM392.Repositories.Base;
using PRM392.Repositories.DbContext;
using PRM392.Repositories.Entities;


namespace PRM392.Repositories
{
    public class UserRoleRepository : GenericRepository<ApplicationRole>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserRoleRepository(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager) : base(context)
        {
            _roleManager = roleManager;
        }

        public async Task<ApplicationRole?> GetRoleByIdAsync(string roleId)
        {
            return await _context.Roles.Where(r => r.Id == roleId).FirstOrDefaultAsync();
        }

        public async Task<ApplicationRole?> GetRoleByNameAsync(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName);
        }

        public async Task<(bool Succeeded, string[] Errors)> CreateRoleAsync(ApplicationRole role)
        {
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description).ToArray());

            role = (await _roleManager.FindByNameAsync(role.Name!))!;

            return (true, Array.Empty<string>());
        }

        public async Task<(bool Succeeded, string[] Errors)> DeleteRoleAsync(ApplicationRole role)
        {
            var result = await _roleManager.DeleteAsync(role);
            return (result.Succeeded, result.Errors.Select(e => e.Description).ToArray());
        }
    }
}
