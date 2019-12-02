using System;
using System.IO;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTOs;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<ProductShopProfile>());

            var destination = @"D:\Entity Framework Core\Exercise JavaScript Object Notation-JSON\01. Import Users_Product Shop\ProductShop\Datasets\";
            var usersLocation = "users.json";
            var categoriesLocation = "categories.json";
            var productsLocation = "products.json";
            var categoriesProducts = "categories-products.json";

            using (var context = new ProductShopContext())
            {
                var inputJson = File
                     .ReadAllText(categoriesProducts);

                Console.WriteLine(GetCategoriesByProductsCount(context));
            }
        }


        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<User[]>(inputJson);
            context.Users.AddRange(users);
            context.SaveChanges();
            return $"Successfully imported {users.Length}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<Product[]>(inputJson);
            context.Products.AddRange(products);
            context.SaveChanges();
            return $"Successfully imported {products.Length}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<Category[]>(inputJson);
            var filtered = categories.Where(x => x.Name != null).ToArray();
            context.Categories.AddRange(filtered);
            context.SaveChanges();
            return $"Successfully imported {filtered.Length}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoriesProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);
            context.CategoryProducts.AddRange(categoriesProducts);
            context.SaveChanges();
            return $"Successfully imported {categoriesProducts.Length}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .Select(x => new
                {
                    name = x.Name,
                    price = x.Price,
                    seller = $"{x.Seller.FirstName} {x.Seller.LastName}"
                })
                .OrderBy(x => x.price);

            var result = JsonConvert.SerializeObject(products, Formatting.Indented);

            return result;
                
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                        .Where(x => x.ProductsSold.Count >= 1 && x.ProductsSold.Any(b => b.Buyer != null))
                        .Select(x => new
                        {
                            firstName = x.FirstName,
                            lastName = x.LastName,
                            soldProducts = x.ProductsSold.Select(y => new
                            {
                                name = y.Name,
                                price = y.Price,
                                buyerFirstName = y.Buyer.FirstName,
                                buyerLastName = y.Buyer.LastName
                            })
                        })
                        .OrderBy(x => x.lastName)
                        .ThenBy(x => x.firstName)
                        .ToList();

            var result = JsonConvert.SerializeObject(users, Formatting.Indented);

            return result;

        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                    .Select(x => new
                    {
                        category = x.Name,
                        productsCount = x.CategoryProducts.Count,
                        averagePrice = $"{x.CategoryProducts.Average(y => y.Product.Price):F2}",
                        totalRevenue = $"{x.CategoryProducts.Sum(y => y.Product.Price):F2}"
                    })
                    .OrderByDescending(x => x.productsCount)
                    .ToList();


            var result = JsonConvert.SerializeObject(categories, Formatting.Indented);

            return result;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context
               .Users
               .Where(u => u.ProductsSold.Any(x => x.Buyer != null))
               .OrderByDescending(u => u.ProductsSold.Count(x => x.Buyer != null))
               .Select(u => new
               {
                   FirstName = u.FirstName,
                   LastName = u.LastName,
                   Age = u.Age,

                   soldProducts = new
                   {
                       Count = u.ProductsSold
                       .Where(p => p.Buyer != null)
                       .Count(),

                       Products = u.ProductsSold
                       .Where(x => x.Buyer != null)
                       .Select(p => new
                       {
                           Name = p.Name,
                           Price = p.Price
                       })
                   }
               })
               .ToList();

            var resultUsers = new
            {
                UsersCount = users.Count,
                Users = users
            };


            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var jsonResult = JsonConvert.SerializeObject(resultUsers, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            return jsonResult;

        }


        //public static string GetUsersWithProducts(ProductShopContext context)
        //{
        //    var users = context.Users
        //          .Include(x => x.ProductsSold)
        //          .ToArray();

        //    var userProducts =
        //        Mapper.Map<User[], UsersSoldProductsDTO[]>(users)
        //         .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
        //        .OrderByDescending(x => x.ProductsSold.Count(y => y.Buyer != null))
        //        .Select(x => new
        //        {
        //            FirstName = x.FirstName,
        //            LastName = x.LastName,
        //            Age = x.Age,

        //            soldProducts = new
        //            {
        //                Count = x.ProductsSold.Count(y => y.Buyer != null),

        //                Products = x.ProductsSold
        //                .Where(b => b.Buyer != null)
        //                .Select(y => new
        //                {
        //                    Name = y.Name,
        //                    Price = y.Price
        //                })
        //            }

        //        })
        //        .ToArray();

        //    var result = new
        //    {
        //        UsersCount = userProducts.Length,
        //        Users = userProducts
        //    };


        //    DefaultContractResolver contractResolver = new DefaultContractResolver()
        //    {
        //        NamingStrategy = new CamelCaseNamingStrategy()
        //    };

        //    var jsonResult = JsonConvert.SerializeObject(result, new JsonSerializerSettings()
        //    {
        //        ContractResolver = contractResolver,
        //        Formatting = Formatting.Indented,
        //        NullValueHandling = NullValueHandling.Ignore
        //    });

        //    return jsonResult;
        //}




    }
}