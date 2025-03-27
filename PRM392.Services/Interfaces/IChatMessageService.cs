using PRM392.Repositories.Models;
using PRM392.Services.DTOs.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services.Interfaces
{
    public interface IChatMessageService
    {
        Task<ApplicationResponse> CreateMessageAsync(CreateChatMessageDTO createChatMessageDTO);
        Task<ApplicationResponse> DeleteMessageAsync(string id);
        Task<ApplicationResponse> GetMessagesAsync();
    }
}
