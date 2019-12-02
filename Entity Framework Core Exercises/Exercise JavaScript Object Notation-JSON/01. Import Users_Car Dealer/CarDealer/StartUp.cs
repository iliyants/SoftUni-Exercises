using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var folderAdress = @"D:\Entity Framework Core\Exercise JavaScript Object Notation-JSON\01. Import Users_Car Dealer\CarDealer\Datasets\";
            var cars = "cars.json";
            var customers = "customers.json";
            var parts = "parts.json";
            var sales = "sales.json";
            var suppliers = "suppliers.json";

            using (var context = new CarDealerContext())
            {
                Mapper.Initialize(cfg => cfg.AddProfile<CarDealerProfile>());
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();
                var inputJson = File.ReadAllText(folderAdress + sales);

                Console.WriteLine(GetSalesWithAppliedDiscount(context));
            }
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<Supplier[]>(inputJson);
            context.Suppliers.AddRange(suppliers);
            int rows = context.SaveChanges();
            return $"Successfully imported {rows}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<Part[]>(inputJson);
            var validParts = new List<Part>();

            var ids = context.Suppliers.Select(x => x.Id).ToList();

            foreach (var part in parts)
            {
                if(!ids.Contains(part.SupplierId))
                {
                    continue;
                }

                validParts.Add(part);
            }

            context.Parts.AddRange(validParts);
            context.SaveChanges();
            return $"Successfully imported {validParts.Count}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carsDTO = JsonConvert.DeserializeObject<ImportCarDTO[]>(inputJson);
            var cars = JsonConvert.DeserializeObject<Car[]>(inputJson);
            context.Cars.AddRange(cars);
            context.SaveChanges();

            foreach (var car in carsDTO)
            {
                var currentCarPartsIds = car.PartsId.ToHashSet();
                var currentCar = context.Cars.Where(x => x.Model == car.Model &&
                x.Make == car.Make && x.TravelledDistance == car.TravelledDistance)
                    .FirstOrDefault();

                foreach (var id in currentCarPartsIds)
                {
                    var partCar = new PartCar()
                    {
                        CarId = currentCar.Id,
                        PartId = id
                    };

                    context.PartCars.Add(partCar);
                }

            }

            context.SaveChanges();
            
            return $"Successfully imported {context.Cars.Count()}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert.DeserializeObject<Customer[]>(inputJson);
            context.Customers.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {context.Customers.Count()}.";
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var sales = JsonConvert.DeserializeObject<Sale[]>(inputJson);
            context.Sales.AddRange(sales);
            context.SaveChanges();
            return $"Successfully imported {context.Sales.Count()}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {

            var customers = context.Customers
                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.IsYoungDriver)
                .Select(x => new Customer
                {
                    Name = x.Name,
                    BirthDate = x.BirthDate,
                    IsYoungDriver = x.IsYoungDriver
                })
                .ProjectTo<CustomerInfoDTO>()
                .ToArray();
            
            var result = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return result;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var toyotas = context.Cars
                .Where(x => x.Make == "Toyota")
                .Select(x => new
                {
                    Id = x.Id,
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ToArray();

            var result = JsonConvert.SerializeObject(toyotas, Formatting.Indented);

            return result;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(x => !x.IsImporter)
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count
                })
                .ToArray();

            var result = JsonConvert.SerializeObject(suppliers, Formatting.Indented);

            return result;

        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carWithParts = context.Cars
                .Select(x => new
                {
                    car = new
                    {
                        Make = x.Make,
                        Model = x.Model,
                        TravelledDistance = x.TravelledDistance
                    },
                    parts = x.PartCars.Select(pc => new
                    {
                        Name = pc.Part.Name,
                        Price = $"{pc.Part.Price:f2}"
                    })
                    .ToArray()
                })
                .ToArray();

            var result = JsonConvert.SerializeObject(carWithParts, Formatting.Indented);

            return result;

        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(x => x.Sales.Any())
                .Select(x => new
                {
                    FullName = x.Name,
                    BoughtCars = x.Sales.Count(),
                    SpentMoney = x.Sales.Sum(y => y.Car.PartCars.Sum(z => z.Part.Price))
                })
                .OrderByDescending(x => x.SpentMoney)
                .ThenByDescending(x => x.BoughtCars)
                .ToArray();


            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var result = JsonConvert.SerializeObject(customers, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });

            return result;

        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(x => new
                {
                    car = new
                    {
                        Make = x.Car.Make,
                        Model = x.Car.Model,
                        TravelledDistance = x.Car.TravelledDistance
                    },

                    customerName = x.Customer.Name,
                    Discount = $"{x.Discount:f2}",
                    price = $"{x.Car.PartCars.Sum(y => y.Part.Price):f2}",
                    priceWithDiscount = $@"{x.Car.PartCars.Sum(y => y.Part.Price)
                    - x.Car.PartCars.Sum(y => y.Part.Price) * (x.Discount / 100):f2}"
                })
                 .Take(10)
                .ToArray();

            var result = JsonConvert.SerializeObject(sales, Formatting.Indented);

            return result;
        }


    }
}