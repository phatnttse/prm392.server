using PRM392.Repositories.Models;
using PRM392.Services.DTOs.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services.Interfaces
{
    public interface INotificationService
    {    
        Task<ApplicationResponse> CreateNotification(NotificationDTO Notification);
        Task<ApplicationResponse> GetNotification(string id);
        Task<ApplicationResponse> UpdateNotification(string id, NotificationDTO Notification);
        Task<ApplicationResponse> DeleteNotification(string id);
        Task<ApplicationResponse> GetNotifications();
    }
}
