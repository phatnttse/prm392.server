using PRM392.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services.DTOs.Notification
{
    public class NotificationDTO
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? UserId { get; set; }
        public bool IsRead { get; set; }
    }
}
