using System.ComponentModel.DataAnnotations;

namespace PRM392.Services.DTOs.Account
{
    public class CreateUserRoleDTO
    {
        [Required(ErrorMessage = "Role name is required"),
         StringLength(200, MinimumLength = 2, ErrorMessage = "Role name must be between 2 and 200 characters")]
        public string? Name { get; set; }

    }
}
