using Microsoft.EntityFrameworkCore;
using PRM392.Repositories.Base;
using PRM392.Repositories.DbContext;
using PRM392.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Repositories
{
    public class ChatMessageRepository : GenericRepository<ChatMessage>
    {
        public ChatMessageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<ChatMessage>> GetChatMessagesAsync(string senderId, string receiverId)
        {
            return await _context.ChatMessages
                .Where(c => (c.SenderId == senderId && c.ReceiverId == receiverId) || (c.SenderId == receiverId && c.ReceiverId == senderId))
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }
}
