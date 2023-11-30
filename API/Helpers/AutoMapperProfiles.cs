using API.DTOs;
using API.Entities;
using API.Entities.API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, UserDto>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<Product, ProductDto>()
              .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<CreateProductDto, Product>()
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Name.Replace(" ", "-").ToLower()))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => new Category { Name = src.Category }));
            CreateMap<UpdateProductDto, Product>();
            CreateMap<Cart, CartDto>();
            CreateMap<CartItem, CartItemDto>();
            CreateMap<Product, CartProductDto>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Images[0]));
            CreateMap<Order, OrderDto>();
            CreateMap<OrderItems, OrderItemDto>()
              .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));
        }
    }
}

