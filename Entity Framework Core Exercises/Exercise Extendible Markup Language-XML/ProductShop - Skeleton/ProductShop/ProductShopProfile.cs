using AutoMapper;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<UserDTO, User>();
            CreateMap<ProductDTO, Product>();
            CreateMap<CategoryDTO, Category>();
            CreateMap<CategoryProductDTO, CategoryProduct>();
            CreateMap<Product, ProductsInRangeDTO>()
                .ForMember(x =>
                x.Buyer, y => y.MapFrom(x => $"{x.Buyer.FirstName} {x.Buyer.LastName}"));
            CreateMap<User, UserSoldProductsDTO>();
        }
    }
}
