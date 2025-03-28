using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services.DTOs.Chat
{
    public class SendChatMessageDTO
    {
        public string? ReceiverId { get; set; }
        public string? Message { get; set; }
        public string? AttachmentUrl { get; set; }

    }
}
