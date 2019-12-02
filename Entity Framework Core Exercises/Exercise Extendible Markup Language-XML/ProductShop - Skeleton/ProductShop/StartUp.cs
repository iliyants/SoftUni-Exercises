using ProductShop.Data;
using System.Linq;
using System;
using System.Xml.Serialization;
using ProductShop.Models;
using System.IO;
using AutoMapper;
using ProductShop.Dtos.Import;
using ProductShop.Dtos.Export;
using System.Text;
using System.Xml;
using AutoMapper.QueryableExtensions;


namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<ProductShopProfile>());

            using (var context = new ProductShopContext())
            {
                var location = @"D:\Entity Framework Core\Exercise Extendible Markup Language-XML\ProductShop - Skeleton\ProductShop\Datasets\";
                var users = "users.xml";
                var products = "products.xml";
                var categories = "categories.xml";
                var categoriesProducts = "categories-products.xml";

                var xmlReader = File.ReadAllText(location + users);

                Console.WriteLine(ImportUsers(context, xmlReader)); 
            }
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var usersDTO = DeserializedCollection<UserDTO>("Users", inputXml);

            var users = Mapper.Map<UserDTO[], User[]>(usersDTO);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {context.Users.Count()}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var productsDTO = DeserializedCollection<ProductDTO>("Products", inputXml);

            var products = Mapper.Map<ProductDTO[], Product[]>(productsDTO);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {context.Products.Count()}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            
            var categoriesDTO = DeserializedCollection<CategoryDTO>("Categories", inputXml);

            var categories = Mapper
                .Map<CategoryDTO[], Category[]>(categoriesDTO)
                .Where(x => x.Name != null);

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {context.Categories.Count()}";

        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var categoryProductsDTO = DeserializedCollection<CategoryProductDTO>("CategoryProducts", inputXml);

            var categoryProducts = Mapper
                .Map<CategoryProductDTO[], CategoryProduct[]>(categoryProductsDTO)
                .Where(x => context.Categories.Any(y => y.Id == x.CategoryId)
                && context.Products.Any(y => y.Id == x.ProductId))
                .ToList();

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();


            return $"Successfully imported {context.CategoryProducts.Count()}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var productsInRange = context
                .Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(x => x.Price)
                .Take(10)
                .ProjectTo<ProductsInRangeDTO>()
                .ToArray();

            return SerializeCollectionToXML("Products", productsInRange);
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(x => x.ProductsSold.Any())
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Take(5)
                .ProjectTo<UserSoldProductsDTO>()
                .ToArray();


            return SerializeCollectionToXML("Users", users);
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context
                .Categories
                .Select(x => new CategoriesByProductsCountDTO
                {
                    Name = x.Name,
                    Count = x.CategoryProducts.Count(),
                    AveragePrice = x.CategoryProducts.Average(y => y.Product.Price),
                    TotalRevenue = x.CategoryProducts.Sum(y => y.Product.Price)
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.TotalRevenue)
                .ToArray();


            return SerializeCollectionToXML("Categories", categories);
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(x => x.ProductsSold.Any())
                .OrderByDescending(x => x.ProductsSold.Count)
                .Select(x => new UsersAndProductsDTO
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Age = x.Age,

                    SoldProducts = new SoldProductsWithCountDTO
                    {
                        Count = x.ProductsSold.Count(),
                        Products = x.ProductsSold
                        .OrderByDescending(p => p.Price)
                        .Select(ps => new SoldProductsDTO
                        {
                            Name = ps.Name,
                            Price = ps.Price
                        }).ToArray()
                    }
                })
                .Take(10)
                .ToArray();

            var result = new GetCountAndUsersDTO
            {
                Count = context.Users.Count(x => x.ProductsSold.Any()),
                Users = users
            };

            return SerializeObjectToXML("Users", result);
        }

        private static T[] DeserializedCollection<T>(string rootAttribute, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(T[]),
                       new XmlRootAttribute(rootAttribute));

            T currentClass = (T)Activator.CreateInstance(typeof(T));

            var typeDeserialization = (T[])serializer
            .Deserialize(new StringReader(inputXml));

            return typeDeserialization;
        }

        private static string SerializeCollectionToXML<T>(string rootAttribute, T[] collection)
        {
            var serializer = new XmlSerializer(typeof(T[]),
                       new XmlRootAttribute(rootAttribute));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), collection, namespaces);

            return sb.ToString().TrimEnd();
        }

        private static string SerializeObjectToXML<T>(string rootAttribute, T currentObject)
        {
            var serializer = new XmlSerializer(typeof(T),
                       new XmlRootAttribute(rootAttribute));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), currentObject, namespaces);

            return sb.ToString().TrimEnd();
        }






    }
}