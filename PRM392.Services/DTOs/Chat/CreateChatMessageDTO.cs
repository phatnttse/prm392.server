using PRM392.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services.DTOs.Chat
{
    public class CreateChatMessageDTO
    {
        public string? Message { get; set; }
        public string? UserId { get; set; }
    }
}
