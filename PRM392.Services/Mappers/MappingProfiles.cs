using AutoMapper;
using PRM392.Repositories.Entities;
using PRM392.Services.DTOs.Cart;
using PRM392.Services.DTOs.Category;
using PRM392.Services.DTOs.Product;

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
        }
    }
}
