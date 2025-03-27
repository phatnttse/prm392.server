using AutoMapper;
using PRM392.Repositories.Entities;
using PRM392.Repositories.Interfaces;
using PRM392.Repositories.Models;
using PRM392.Services.DTOs.Notification;
using PRM392.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApplicationResponse> CreateNotification(NotificationDTO notificationDTO)
        {
            try
            {
                var notification = _mapper.Map<Notification>(notificationDTO);
                await _unitOfWork.NotificationRepository.AddAsync(notification);
                return new ApplicationResponse
                {
                    Success = true,
                    Message = "Create notification successfully",
                    Data = notificationDTO,
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

        public async Task<ApplicationResponse> DeleteNotification(string id)
        {
            try
            {
                var notification = await _unitOfWork.NotificationRepository.GetByIdAsync(id);
                if (notification == null)
                {
                    throw new ApiException("Notification not found", System.Net.HttpStatusCode.NotFound);  
                }
                    _unitOfWork.NotificationRepository.Delete(notification);
                return new ApplicationResponse
                {
                    Success = true,
                    Message = "Create notification successfully",
                    Data = notification,
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

        public async Task<ApplicationResponse> GetNotification(string id)
        {
            try
            {
                var notification = await _unitOfWork.NotificationRepository.GetByIdAsync(id);
                if (notification == null)
                {
                    throw new ApiException("Notification not found", System.Net.HttpStatusCode.NotFound);
                }
                return new ApplicationResponse
                {
                    Success = true,
                    Message = "Get notification successfully",
                    Data = notification,
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

        public async Task<ApplicationResponse> GetNotifications()
        {
            try
            {
                var listNotification = await _unitOfWork.NotificationRepository.GetAllAsync();
                return new ApplicationResponse
                {
                    Success = true,
                    Message = "Get notifications successfully",
                    Data = listNotification,
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

        public async Task<ApplicationResponse> UpdateNotification(string id, NotificationDTO notification)
        {
            try
            {
                var notificationExisted = await _unitOfWork.NotificationRepository.GetByIdAsync(id);
                if (notificationExisted == null)
                {
                    throw new ApiException("Notification not found", System.Net.HttpStatusCode.NotFound);
                }
                _mapper.Map(notification, notificationExisted);
                _unitOfWork.NotificationRepository.Update(notificationExisted);
                return new ApplicationResponse
                {
                    Success = true,
                    Message = "Update notification successfully",
                    Data = notification,
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
