using PRM392.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Repositories.Entities
{
    public class ChatMessage : BaseEntity
    {
        public string? Message { get; set; }
        public string? SenderId { get; set; }
        public virtual ApplicationUser? Sender { get; set; }
        public string? ReceiverId { get; set; }
        public virtual ApplicationUser? Receiver { get; set; }
        public string? AttachmentUrl { get; set; }  
        public bool IsRead { get; set; }  

    }
}
