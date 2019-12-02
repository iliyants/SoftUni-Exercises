using AutoMapper;
using CarDealer.Dtos.Export;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using System.Linq;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<ImportSupplierDTO, Supplier>();
            CreateMap<ImportPartDTO, Part>();
            CreateMap<ImportCarDTO, Car>();
            CreateMap<ImportCustomerDTO, Customer>();
            CreateMap<ImportSaleDTO, Sale>();
            CreateMap<Car, ExportCarDTO>();



            CreateMap<Sale, ExportSalewithAppliedDiscount>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
                .ForMember(dest => dest.Price,
                opt => opt.MapFrom(src => src.Car.PartCars.Sum(pc => pc.Part.Price)))
                .ForMember(dest => dest.PriceWithDiscount,
                opt => opt.MapFrom(src =>
                    src.Car.PartCars.Sum(pc => pc.Part.Price)
                    - (src.Car.PartCars.Sum(pc => pc.Part.Price) * src.Discount / 100)));

        }

    }
}
