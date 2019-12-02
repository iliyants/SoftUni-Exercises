using AutoMapper;
using ProductShop.DTOs;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<User, UsersSoldProductsDTO>();

            CreateMap<Product, ProductDTO>()
            .ForMember(x => x.Buyer, y => y.MapFrom(m => m.Buyer.FirstName));
        }
    }
}
