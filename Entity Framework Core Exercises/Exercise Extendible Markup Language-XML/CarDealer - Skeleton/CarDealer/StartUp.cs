using AutoMapper;
using CarDealer.Data;
using System.IO;
using System;
using CarDealer.Helpers;
using CarDealer.Dtos.Import;
using AutoMapper.QueryableExtensions;
using CarDealer.Models;
using System.Linq;
using CarDealer.Dtos.Export;
using static CarDealer.Helpers.XMLSerializationHelper;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<CarDealerProfile>());

            var location = @"D:\Entity Framework Core\Exercise Extendible Markup Language-XML\CarDealer - Skeleton\CarDealer\Datasets\";
            var cars = "cars.xml";
            var customers = "customers.xml";
            var parts = "parts.xml";
            var sales = "sales.xml";
            var suppliers = "suppliers.xml";

            using (var context = new CarDealerContext())
            {

                var xmlReader = File.ReadAllText(location + suppliers);

                Console.WriteLine(ImportSuppliers(context,xmlReader)); 
            }
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var suppliersDTO = XMLSerializationHelper
                 .DeserializedCollection<ImportSupplierDTO>("Suppliers", inputXml);
            var suppliers = Mapper.Map<ImportSupplierDTO[], Supplier[]>(suppliersDTO);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var partsDTO = XMLSerializationHelper
                .DeserializedCollection<ImportPartDTO>("Parts", inputXml)
                .Where(x => context.Suppliers.Any(y => y.Id == x.SupplierId))
                .ToArray();

            var parts = Mapper.Map<ImportPartDTO[], Part[]>(partsDTO);

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Length}";

        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var carsDto = XMLSerializationHelper
                .DeserializedCollection<ImportCarDTO>("Cars", inputXml);

            var cars = Mapper.Map<ImportCarDTO[], Car[]>(carsDto);
            context.Cars.AddRange(cars);
            context.SaveChanges();

            var carsWithPartIds = XMLSerializationHelper
                .DeserializedCollection<ImportCarWIthPartIdsDTO>("Cars", inputXml);


            foreach (var car in carsWithPartIds)
            {
                var currentCar = context.Cars.FirstOrDefault(x => x.Make == car.Make
                && x.Model == car.Model && x.TravelledDistance == car.TravelledDistance);


                var currentPartIds = car.PartIds
                    .Where(x => context.Parts.Any(y => y.Id == x.Id))
                    .Select(x => x.Id)
                    .ToHashSet();

                foreach (var id in currentPartIds)
                {
                    var partCar = new PartCar
                    {
                        CarId = currentCar.Id,
                        PartId = id
                    };

                    context.PartCars.Add(partCar);
                }                
            }

            context.SaveChanges();

            
            return $"Successfully imported {context.Cars.Count()}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var customersDto = XMLSerializationHelper
                .DeserializedCollection<ImportCustomerDTO>("Customers", inputXml);

            var customers = Mapper.Map<ImportCustomerDTO[], Customer[]>(customersDto);

            context.Customers.AddRange(customers);
                context.SaveChanges();

            return $"Successfully imported {context.Customers.Count()}";
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {

            var salesDto = XMLSerializationHelper
                .DeserializedCollection<ImportSaleDTO>("Sales", inputXml)
                .Where(x => context.Cars.Any(y => y.Id == x.CarId))
                .ToArray();

            var sales = Mapper.Map<ImportSaleDTO[], Sale[]>(salesDto);


            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {context.Sales.Count()}";
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(x => x.TravelledDistance > 2000000)
                .OrderBy(x => x.Make)
                .ThenBy(x => x.Model)
                .ProjectTo<ExportCarDTO>()
                .Take(10)
                .ToArray();

            return XMLSerializationHelper
                .SerializeCollectionToXML("cars", cars);
                
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var carsBmw = context.Cars
              .Where(x => x.Make == "BMW")
              .OrderBy(x => x.Model)
              .ThenByDescending(x => x.TravelledDistance)
              .Select(x => new ExportCarMakeBmwDTO
              {
                  Id = x.Id,
                  Model = x.Model,
                  TravelledDistance = x.TravelledDistance
              })
              .ToArray();

            return XMLSerializationHelper
                .SerializeCollectionToXML("cars", carsBmw);
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var localSuppliers = context.Suppliers
                .Where(x => !x.IsImporter)
                .Select(x => new ExportLocalSupplierDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count()
                })
                .ToArray();

            return XMLSerializationHelper
                .SerializeCollectionToXML("suppliers", localSuppliers);
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carParts = context.Cars
                .Select(x => new ExportCarsWithTheirListOfPartsDTO
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance,
                    Parts = x.PartCars.Select(y => new ExportPartDTO
                    {
                        Name = y.Part.Name,
                        Price = y.Part.Price
                    })
                    .OrderByDescending(p => p.Price)
                    .ToArray()
                })
                .OrderByDescending(x => x.TravelledDistance)
                .ThenBy(x => x.Model)
                .Take(5)
                .ToArray();

            return XMLSerializationHelper
                .SerializeCollectionToXML("cars", carParts);


        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(x => x.Sales.Any())
                .Select(x => new ExportTotalSalesByCustomerDTO
                {
                    FullName = x.Name,
                    BoughtCars = x.Sales.Count(),
                    SpentMoney = x.Sales.Sum(sale => sale.Car.PartCars.Sum(pc => pc.Part.Price))
                })
                .OrderByDescending(x => x.SpentMoney)
                .ToArray();

            return XMLSerializationHelper
                .SerializeCollectionToXML("customers", customers);
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(x => new ExportSalewithAppliedDiscount
                {
                    Car = new ExportCarWithAttributesDTO
                    {
                        Make = x.Car.Make,
                        Model = x.Car.Model,
                        TravelledDistance = x.Car.TravelledDistance
                    },
                    Discount = x.Discount,
                    CustomerName = x.Customer.Name,
                    Price = x.Car.PartCars.Sum(pc => pc.Part.Price),
                    PriceWithDiscount = x.Car.PartCars.Sum(y => y.Part.Price)
                    - (x.Car.PartCars.Sum(y => y.Part.Price) * x.Discount / 100)
                })
                .ToArray();


            return XMLSerializationHelper
                .SerializeCollectionToXML("sales", sales);
        }





    }
}