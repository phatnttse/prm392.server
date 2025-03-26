using Microsoft.AspNetCore.Identity;
using PRM392.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Repositories.Entities
{
    public class ApplicationRole : IdentityRole, IAuditableEntity
    {
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        /// <summary>
        /// Navigation property for the users in this role.
        /// </summary>
        public ICollection<IdentityUserRole<string>> Users { get; } = new List<IdentityUserRole<string>>();

        /// <summary>
        /// Navigation property for claims in this role.
        /// </summary>
        public ICollection<IdentityRoleClaim<string>> Claims { get; } = new List<IdentityRoleClaim<string>>();
    }
}
