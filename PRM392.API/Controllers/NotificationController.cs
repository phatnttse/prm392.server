using Microsoft.AspNetCore.Mvc;
using PRM392.Services.DTOs.Notification;
using PRM392.Services.Interfaces;

namespace PRM392.API.Controllers
{
    /// <summary>
    /// Controller for handling notification-related operations.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/notification")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationController"/> class.
        /// </summary>
        /// <param name="notificationService">The notification service.</param>
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Gets all notifications.
        /// </summary>
        /// <returns>A list of notifications.</returns>
        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            return Ok(await _notificationService.GetNotifications());
        }

        /// <summary>
        /// Creates a new notification.
        /// </summary>
        /// <param name="notificationDTO">The notification data transfer object.</param>
        /// <returns>The created notification.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] NotificationDTO notificationDTO)
        {
            return Ok(await _notificationService.CreateNotification(notificationDTO));
        }

        /// <summary>
        /// Gets a notification by its identifier.
        /// </summary>
        /// <param name="id">The notification identifier.</param>
        /// <returns>The notification with the specified identifier.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationById(string id)
        {
            return Ok(await _notificationService.GetNotification(id));
        }

        /// <summary>
        /// Deletes a notification by its identifier.
        /// </summary>
        /// <param name="id">The notification identifier.</param>
        /// <returns>The result of the delete operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(string id)
        {
            return Ok(await _notificationService.DeleteNotification(id));
        }

        /// <summary>
        /// Updates a notification by its identifier.
        /// </summary>
        /// <param name="id">The notification identifier.</param>
        /// <param name="notificationDTO">The notification data transfer object.</param>
        /// <returns>The updated notification.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification(string id, [FromBody] NotificationDTO notificationDTO)
        {
            return Ok(await _notificationService.UpdateNotification(id, notificationDTO));
        }
    }
}
