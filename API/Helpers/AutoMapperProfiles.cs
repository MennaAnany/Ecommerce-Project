using API.DTOs;
using API.Entities;
using AutoMapper;
using System.Text.RegularExpressions;

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
        }
    }
}

