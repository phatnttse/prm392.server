using Microsoft.AspNetCore.Identity;
using PRM392.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Repositories.Entities
{
    public class ApplicationUser : IdentityUser, IAuditableEntity
    {
        public string? FullName { get; set; }
        public string? PictureUrl { get; set; }
        public string? PictureFileName { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        /// <summary>
        /// Navigation property for the roles this user belongs to.
        /// </summary>
        public ICollection<IdentityUserRole<string>> Roles { get; } = new List<IdentityUserRole<string>>();

        /// <summary>
        /// Navigation property for the claims this user possesses.
        /// </summary>
        public ICollection<IdentityUserClaim<string>> Claims { get; } = new List<IdentityUserClaim<string>>();

    }
}
