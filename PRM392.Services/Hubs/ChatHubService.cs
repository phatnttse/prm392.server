using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PRM392.Repositories.Entities;
using PRM392.Repositories.Enums;
using PRM392.Repositories.Interfaces;
using PRM392.Repositories.Models;
using PRM392.Services.DTOs.Chat;
using PRM392.Utils;
using System.Collections.Concurrent;


namespace PRM392.Services.Hubs
{
    [Authorize]
    public sealed class ChatHubService : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ChatHubService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public static readonly ConcurrentDictionary<string, OnlineUserDTO> onlineUsers = new();

        public override async Task OnConnectedAsync()
        {
            try
            {
                string currentUserId = Utilities.GetCurrentUserId() ?? throw new ApiException("User not found", System.Net.HttpStatusCode.NotFound);

                ApplicationUser currentUser = await _unitOfWork.UserAccountRepository.GetByIdAsync(currentUserId!) ?? throw new ApiException("User not found", System.Net.HttpStatusCode.NotFound);

                var connectionId = Context.ConnectionId;

                if (onlineUsers.ContainsKey(currentUserId!))
                {
                    onlineUsers[currentUserId!].ConnectionId = connectionId;
                }
                else
                {
                    var user = new OnlineUserDTO
                    {
                        ConnectionId = connectionId,
                        Id = currentUser?.Id,
                        UserName = currentUser?.UserName,
                        FullName = currentUser?.FullName,
                        PictureUrl = currentUser?.PictureUrl
                    };

                    onlineUsers.TryAdd(currentUserId!, user);

                    await Clients.AllExcept(connectionId).SendAsync("UserConnected", user);
                }

                await Clients.All.SendAsync("OnlineUsers", this.GetAllOnlineUsers());

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

        public async Task SendMessage(SendChatMessageDTO body)
        {
            try
            {
                string senderId = Utilities.GetCurrentUserId() ?? throw new ApiException("Sender not found", System.Net.HttpStatusCode.NotFound);

                ChatMessage chatMessage = new ChatMessage
                {
                    SenderId = senderId,
                    ReceiverId = body.ReceiverId,
                    Message = body.Message,
                    AttachmentUrl = body.AttachmentUrl,
                    IsRead = false,
                    ActiveFlag = (byte)ActiveFlag.Active
                };

                await _unitOfWork.ChatMessageRepository.AddAsync(chatMessage);

                await _unitOfWork.SaveChangesAsync();

                var connectionId = onlineUsers.Values.FirstOrDefault(u => u.Id == body.ReceiverId)?.ConnectionId;

                if (connectionId != null)
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveNewMessage", _mapper.Map<MessageDTO>(chatMessage));
                }

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

        public async Task LoadMessages(string recipientId, int pageNumber = 1)
        {
            try
            {
                //int pageSize = 20;

                string currentUserId = Utilities.GetCurrentUserId() ?? throw new ApiException("User not found", System.Net.HttpStatusCode.NotFound);

                ApplicationUser currentUser = await _unitOfWork.UserAccountRepository.GetByIdAsync(currentUserId!) ?? throw new ApiException("User not found", System.Net.HttpStatusCode.NotFound);

                List<ChatMessage> messages = await _unitOfWork.ChatMessageRepository.GetChatMessagesAsync(currentUserId, recipientId);

                foreach (var message in messages)
                {
                    var msg = await _unitOfWork.ChatMessageRepository.GetByIdAsync(message.Id);

                    if (msg != null && msg.ReceiverId == currentUserId)
                    {
                        msg.IsRead = true;

                        _unitOfWork.ChatMessageRepository.Update(msg);

                        await _unitOfWork.SaveChangesAsync();
                    }
                }

                await Clients.Caller.SendAsync("ReceiveMessageList", _mapper.Map<List<MessageDTO>>(messages));

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

        public async Task NotifyTyping(string recipientId)
        {
            try
            {
                string senderId = Utilities.GetCurrentUserId() ?? throw new ApiException("Sender not found", System.Net.HttpStatusCode.NotFound);

                ApplicationUser sender = await _unitOfWork.UserAccountRepository.GetByIdAsync(senderId!) ?? throw new ApiException("User not found", System.Net.HttpStatusCode.NotFound);

                var connectionId = onlineUsers.Values.FirstOrDefault(u => u.Id == recipientId)?.ConnectionId;

                if (connectionId != null)
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveTypingNotification", sender != null ? sender.FullName : "Unknow");
                }
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

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                string currentUserId = Utilities.GetCurrentUserId() ?? throw new ApiException("User not found", System.Net.HttpStatusCode.NotFound);
                onlineUsers.Remove(currentUserId!, out _);
                await Clients.All.SendAsync("OnlineUsers", this.GetAllOnlineUsers());
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


        private List<OnlineUserDTO> GetAllOnlineUsers()
        {
            try
            {
                string currentUserId = Utilities.GetCurrentUserId() ?? throw new ApiException("User not found", System.Net.HttpStatusCode.NotFound);

                HashSet<string> onlineUsersSet = onlineUsers != null ? new HashSet<string>(onlineUsers.Keys) : new HashSet<string>();

                List<ApplicationUser> users = _unitOfWork.UserAccountRepository.GetAllUsersAsync() ?? new List<ApplicationUser>();

                List<OnlineUserDTO> onlineUserDTOs = users.Select(u => new OnlineUserDTO
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FullName = u.FullName,
                    PictureUrl = u.PictureUrl,
                    IsOnline = onlineUsersSet.Contains(u.Id)
                }).ToList();

                return onlineUserDTOs;
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
