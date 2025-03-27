using AutoMapper;
using PRM392.Repositories.Entities;
using PRM392.Services.DTOs.Cart;
using PRM392.Services.DTOs.Category;
using PRM392.Services.DTOs.Product;
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
            //Category
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<CreateUpdateCategoryDTO, Category>().ReverseMap()
                 .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            //Product
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<CreateUpdateProductDTO, Product>().ReverseMap()
                 .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            //CartItem
            CreateMap<CartItem, CartItemDTO>().ReverseMap();

            CreateMap<StoreLocationDTO, StoreLocation>();
            CreateMap<StoreLocation, StoreLocationDTO>(); // Nếu cần ánh xạ ngược
        }
    }
}
