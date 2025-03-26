using PRM392.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Repositories.Entities
{
    public class Notification : BaseEntity
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public bool IsRead { get; set; }
    }
}
