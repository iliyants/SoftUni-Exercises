using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FastFood.Data;
using System.Linq;
using FastFood.Models.Enums;
using System.Collections.Generic;
using FastFood.DataProcessor.Dto.Export;
using Newtonsoft.Json;

namespace FastFood.DataProcessor
{
    public class Serializer
    {
        public static string ExportOrdersByEmployee(FastFoodDbContext context, string employeeName, string orderType)
        {
            var orders = context.Employees
                .Where(x => x.Name == employeeName)
                .Select(x => new
                {
                    Name = x.Name,

                    Orders = x.Orders
                    .Where(o => o.Type.ToString() == orderType)
                    .Select(o => new
                    {
                        Customer = o.Customer,

                        Items = o.OrderItems.Select(i => new
                        {
                            Name = i.Item.Name,
                            Price = i.Item.Price,
                            Quantity = i.Quantity
                        })
                        .ToHashSet(),

                        TotalPrice = o.TotalPrice
                    })
                    .OrderByDescending(x => x.TotalPrice)
                    .ThenByDescending(x => x.Items.Count())
                    .ToHashSet(),

                    TotalMade = x.Orders
                        .Sum(x => x.TotalPrice)
                })
                .FirstOrDefault();

            var jsonString = JsonConvert.SerializeObject(orders, Newtonsoft.Json.Formatting.Indented);

            return jsonString;

        }

        public static string ExportCategoryStatistics(FastFoodDbContext context, string categoriesString)
        {
            var categories = categoriesString.Split(',').ToArray();

            var result = new List<ExportCategoryDTO>();

            foreach (var categoryName in categories)
            {
                var category = context
                    .Categories
                    .Where(x => x.Name == categoryName)
                    .Select(x => new ExportCategoryDTO()
                    {
                        Name = x.Name,

                        Item = x.Items.Select(i => new ExportItemDTO
                        {
                            Name = i.Name,
                            TotalMade = i.Price * i.OrderItems.Sum(oi => oi.Quantity),
                            TimesSold = i.OrderItems.Sum(oi => oi.Quantity)
                        })
                        .OrderByDescending(x => x.TotalMade)
                        .FirstOrDefault()
                    }).FirstOrDefault();

                result.Add(category);
            }

            return SerializeCollectionToXML("Categories", result.OrderByDescending(x => x.Item.TotalMade)
                .ThenByDescending(x => x.Item.TimesSold)
                .ToArray());
        }



        public static string SerializeCollectionToXML<T>(string rootAttribute, T[] collection)
        {
            var serializer = new XmlSerializer(typeof(T[]),
                       new XmlRootAttribute(rootAttribute));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), collection, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string SerializeObjectToXML<T>(string rootAttribute, T currentObject)
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