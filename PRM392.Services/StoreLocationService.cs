using AutoMapper;
using PRM392.Repositories.Entities;
using PRM392.Repositories.Interfaces;
using PRM392.Repositories.Models;
using PRM392.Services.DTOs.StoreLocation;
using PRM392.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services
{
    public class StoreLocationService : IStoreLocationService
    {
        private readonly IStoreLocation _storeLocationRepository;
        private readonly IMapper _mapper;

        public StoreLocationService(IStoreLocation storeLocationRepository, IMapper mapper)
        {
            _storeLocationRepository = storeLocationRepository;
            _mapper = mapper;
        }

        public async Task<ApplicationResponse> AddNewLocation(StoreLocationDTO storeLocationDTO)
        {
            try
            {
                var storeLocation = _mapper.Map<StoreLocation>(storeLocationDTO);
                await _storeLocationRepository.AddNewStoreLocation(storeLocation);
                return new ApplicationResponse
                {
                    Success = true,
                    Message = "Add new store location successfully!",
                    Data = storeLocationDTO,
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

        public async Task<ApplicationResponse> DeleteLocationAsync(decimal? latitute, decimal? longitute)
        {
            try
            {
                if (latitute.HasValue && longitute.HasValue)
                {
                    var deletedLocation = await _storeLocationRepository.RemoveStoreLocation(latitute.Value, longitute.Value);
                    return new ApplicationResponse
                    {
                        Success = true,
                        Message = "Store location deleted successfully!",
                        Data = deletedLocation,
                        StatusCode = System.Net.HttpStatusCode.OK
                    };
                }
                else
                {
                    return new ApplicationResponse
                    {
                        Success = false,
                        Message = "Latitude and Longitude must have values.",
                        StatusCode = System.Net.HttpStatusCode.BadRequest
                    };
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

        public async Task<ApplicationResponse> GetStoreLocationsAsync()
        {
            try
            {
                var listStoreLocation = await _storeLocationRepository.GetAllStoreLocations();
                var listStoreLocationDTO = _mapper.Map<List<StoreLocationDTO>>(listStoreLocation);
                return new ApplicationResponse
                {
                    Success = true,
                    Message = "Get all store locations successfully hihi!",
                    Data = listStoreLocationDTO,
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
