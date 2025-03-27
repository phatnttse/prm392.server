using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRM392.Services.DTOs.StoreLocation;
using PRM392.Services.Interfaces;

namespace PRM392.API.Controllers
{
    /// <summary>
    /// Controller for managing store locations.
    /// </summary>
    /// 
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/store-location")]

    public class StoreLocationsController : ControllerBase
    {
        private readonly IStoreLocationService _storeLocationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreLocationsController"/> class.
        /// </summary>
        /// <param name="storeLocationService">The store location service.</param>
        public StoreLocationsController(IStoreLocationService storeLocationService)
        {
            _storeLocationService = storeLocationService;
        }

        /// <summary>
        /// Gets the list of store locations.
        /// </summary>
        /// <returns>A list of store locations.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetStoreLocations()
        {
            return Ok(await _storeLocationService.GetStoreLocationsAsync());
        }

        /// <summary>
        /// Adds a new store location.
        /// </summary>
        /// <param name="storeLocationDTO">The store location data transfer object.</param>
        /// <returns>The result of the add operation.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddNewLocation([FromBody] StoreLocationDTO storeLocationDTO)
        {
            return Ok(await _storeLocationService.AddNewLocation(storeLocationDTO));
        }

        /// <summary>
        /// Deletes a store location by latitude and longitude.
        /// </summary>
        /// <param name="latitude">The latitude of the store location.</param>
        /// <param name="longitude">The longitude of the store location.</param>
        /// <returns>The result of the delete operation.</returns>
        [HttpDelete("{latitude}/{longitude}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteLocation(decimal latitude, decimal longitude)
        {
            return Ok(await _storeLocationService.DeleteLocationAsync(latitude, longitude));
        }
    }
}
