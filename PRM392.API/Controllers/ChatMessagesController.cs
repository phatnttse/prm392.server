using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRM392.Services.DTOs.Chat;
using PRM392.Services.Interfaces;

namespace PRM392.API.Controllers
{
    /// <summary>
    /// Controller for managing chat messages.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/chat-message")]
    public class ChatMessagesController : ControllerBase
    {
        private readonly IChatMessageService _chatMessageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessagesController"/> class.
        /// </summary>
        /// <param name="chatMessageService">The chat message service.</param>
        public ChatMessagesController(IChatMessageService chatMessageService)
        {
            _chatMessageService = chatMessageService;
        }

        /// <summary>
        /// Gets all chat messages.
        /// </summary>
        /// <returns>A list of chat messages.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMessages()
        {
            return Ok(await _chatMessageService.GetMessagesAsync());
        }

        /// <summary>
        /// Creates a new chat message.
        /// </summary>
        /// <param name="createChatMessageDTO">The chat message DTO.</param>
        /// <returns>The created chat message.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMessage([FromBody] CreateChatMessageDTO createChatMessageDTO)
        {
            return Ok(await _chatMessageService.CreateMessageAsync(createChatMessageDTO));
        }

        /// <summary>
        /// Deletes a chat message by ID.
        /// </summary>
        /// <param name="id">The ID of the chat message to delete.</param>
        /// <returns>The result of the delete operation.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteMessage(string id)
        {
            return Ok(await _chatMessageService.DeleteMessageAsync(id));
        }
    }
}
