using AutoMapper;
using PRM392.Repositories.Entities;
using PRM392.Services.DTOs.Cart;
using PRM392.Services.DTOs.Category;
using PRM392.Services.DTOs.Product;
using PRM392.Services.DTOs.StoreLocation;
using PRM392.Services.DTOs.Chat;
using PRM392.Services.DTOs.Notification;
using PRM392.Services.DTOs.Order;
using PRM392.Services.DTOs.Account;

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
            CreateMap<ProductImage, ProductImageDTO>().ReverseMap();
            CreateMap<ProductDTO, Product>().ReverseMap();

            //CartItem
            CreateMap<CartItem, CartItemDTO>().ReverseMap();

            CreateMap<StoreLocationDTO, StoreLocation>();
            CreateMap<StoreLocation, StoreLocationDTO>(); // Nếu cần ánh xạ ngược

            //Chat
            CreateMap<CreateChatMessageDTO, ChatMessage>();
            CreateMap<ChatMessage, CreateChatMessageDTO>(); // Nếu cần ánh xạ ngược

            //Notification
            CreateMap<Notification, NotificationDTO>().ReverseMap();
            CreateMap<NotificationDTO, Notification>().ReverseMap();

            //Order
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<CreateOrderDTO, Order>().ReverseMap()
                 .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();

            // UserAccount
            CreateMap<ApplicationUser, UserAccountDTO>().ReverseMap();

            //Message
            CreateMap<ChatMessage, MessageDTO>().ReverseMap();
        }
    }
}
