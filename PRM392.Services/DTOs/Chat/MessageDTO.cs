using PRM392.Services.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services.DTOs.Chat
{
    public class MessageDTO
    {
        public string? Id { get; set; }
        public virtual UserAccountDTO? Sender { get; set; }
        public virtual UserAccountDTO? Receiver { get; set; }
        public string? Message { get; set; }
        public string? AttachmentUrl { get; set; }
        public bool IsRead { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
