using AutoMapper;
using PRM392.Repositories.Entities;
using PRM392.Repositories.Interfaces;
using PRM392.Repositories.Models;
using PRM392.Services.DTOs.Chat;
using PRM392.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ChatMessageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApplicationResponse> CreateMessageAsync(CreateChatMessageDTO createChatMessageDTO)
        {
            try
            {
                var chatMessage = _mapper.Map<ChatMessage>(createChatMessageDTO);
                await _unitOfWork.ChatMessageRepository.AddAsync(chatMessage);
                return new ApplicationResponse
                {
                    Success = true,
                    Message = "Create message successfully",
                    Data = createChatMessageDTO,
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ApplicationResponse> DeleteMessageAsync(string id)
        {
            try
            {
                var message = await _unitOfWork.ChatMessageRepository.GetByIdAsync(id);
                _unitOfWork.ChatMessageRepository.Delete(message);
                return new ApplicationResponse
                {
                    Success = true,
                    Message = "Delete message successfully!",
                    Data = message,
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ApplicationResponse> GetMessagesAsync()
        {
            try
            {
                var listMessages = await _unitOfWork.ChatMessageRepository.GetAllAsync();
                return new ApplicationResponse
                {
                    Success = true,
                    Message = "Get messages successfully!",
                    Data = listMessages,
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
