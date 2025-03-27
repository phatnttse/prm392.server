using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRM392.Services.DTOs.StoreLocation;
using PRM392.Services.Interfaces;

namespace PRM392.API.Controllers
{
    [Route("api/v{version:apiVersion}/store-location")]
    [ApiController]
    public class StoreLocationsController : ControllerBase
    {
        private readonly IStoreLocationService _storeLocationService;
        public StoreLocationsController(IStoreLocationService storeLocationService)
        {
            _storeLocationService = storeLocationService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetStoreLocations()
        {
            return Ok(await _storeLocationService.GetStoreLocationsAsync());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddNewLocation([FromBody] StoreLocationDTO storeLocationDTO)
        {
            return Ok(await _storeLocationService.AddNewLocation(storeLocationDTO));
        }

        [HttpDelete("{latitude}/{longitude}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteLocation(decimal latitude, decimal longitude)
        {
            return Ok(await _storeLocationService.DeleteLocationAsync(latitude, longitude));
        }
    }
}
