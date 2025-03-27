using AutoMapper;
using PRM392.Repositories.Entities;
using PRM392.Services.DTOs.StoreLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services.Mappers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<StoreLocationDTO, StoreLocation>();
            CreateMap<StoreLocation, StoreLocationDTO>(); // Nếu cần ánh xạ ngược
        }
    }
}
