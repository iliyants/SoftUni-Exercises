using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using FastFood.Data;
using FastFood.DataProcessor.Dto.Import;
using FastFood.Models;
using FastFood.Models.Enums;
using Newtonsoft.Json;

namespace FastFood.DataProcessor
{
	public static class Deserializer
	{
		private const string FailureMessage = "Invalid data format.";
		private const string SuccessMessage = "Record {0} successfully imported.";

		public static string ImportEmployees(FastFoodDbContext context, string jsonString)
		{
            var employeesDTO = JsonConvert.DeserializeObject<ImportEmployeeDTO[]>(jsonString);

            var sb = new StringBuilder();

            foreach (var employee in employeesDTO)
            {
                var validModel = IsValid(employee);

                if (!validModel)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var position = context.Positions.FirstOrDefault(x => x.Name == employee.Position)
                    ?? CreatePosition(context, employee.Position);

                var currentEmployee = new Employee()
                {
                    Name = employee.Name,
                    Position = position
                };

                context.Employees.Add(currentEmployee);

                sb.AppendLine(string.Format(SuccessMessage, currentEmployee.Name));
            }

            context.SaveChanges();
            return sb.ToString();

        }


        public static string ImportItems(FastFoodDbContext context, string jsonString)
		{
            var itemsDTO = JsonConvert.DeserializeObject<ImportItemDTO[]>(jsonString);

            var sb = new StringBuilder();

            foreach (var itemDTO in itemsDTO)
            {
                var validModel = IsValid(itemDTO);
                var itemAlreadyExists = context.Items.FirstOrDefault(x => x.Name == itemDTO.Name);

                if (!validModel || itemAlreadyExists != null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var category = context.Categories.FirstOrDefault(x => x.Name == itemDTO.Category)
                    ?? CreateCategory(context, itemDTO.Category);

                var item = new Item()
                {
                    Name = itemDTO.Name,
                    Price = itemDTO.Price,
                    Category = category
                };

                context.Items.Add(item);

                sb.AppendLine(string.Format(SuccessMessage, item.Name));
                context.SaveChanges();

            }

            return sb.ToString();

        }


        public static string ImportOrders(FastFoodDbContext context, string xmlString)
		{
            var orderItemsDTO = DeserializedCollection<ImportOrderDTO>("Orders", xmlString);

            var sb = new StringBuilder();

            foreach (var orderItem in orderItemsDTO)
            {
                var employeeExists = context.Employees.Any(x => x.Name == orderItem.EmployeeName);
                var allOfTheItemsExist = ChecksItemExistance(context, orderItem.Items);
                var isOrderValid = IsValid(orderItem);
                var areItemsValid = orderItem.Items.All(IsValid);
                var isOrderTypeValid = Enum.TryParse(orderItem.OrderType, out OrderType type);

                if (!employeeExists || !allOfTheItemsExist || !isOrderValid || !areItemsValid || !isOrderTypeValid)
                {
                    continue;
                }

                var employee = context.Employees.FirstOrDefault(x => x.Name == orderItem.EmployeeName);
                var order = new Order()
                {
                    Customer = orderItem.CustomerName,
                    DateTime = DateTime.ParseExact(orderItem.DateTime, "dd/MM/yyyy HH:mm",
                    CultureInfo.InvariantCulture),
                    Employee = employee,
                    Type = (OrderType)Enum.Parse(typeof(OrderType), orderItem.OrderType),
                };

                context.Orders.Add(order);
                context.SaveChanges();


                foreach (var item in orderItem.Items)
                {
                    var currentItem = context.Items.FirstOrDefault(x => x.Name == item.Name);
                    var currentOrderItem = new OrderItem()
                    {
                        Order = order,
                        Item = currentItem,
                        Quantity = item.Quantity
                    };

                    context.OrderItems.Add(currentOrderItem);
                    context.SaveChanges();
                }

                sb.AppendLine($"Order for {orderItem.CustomerName} on {order.DateTime.ToString("dd/MM/yyyy HH:mm")} added");
            }

            return sb.ToString();
		}

        private static bool ChecksItemExistance(FastFoodDbContext context, ImportItemQuantityDTO[] items)
        {
            foreach (var item in items)
            {
                if (!context.Items.Any(x => x.Name == item.Name))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsValid(object model)
        {
            var validationContext = new ValidationContext(model);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(model, validationContext, validationResult, true);
        }

        public static T[] DeserializedCollection<T>(string rootAttribute, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(T[]),
                       new XmlRootAttribute(rootAttribute));

            T currentClass = (T)Activator.CreateInstance(typeof(T));

            var typeDeserialization = (T[])serializer
            .Deserialize(new StringReader(inputXml));

            return typeDeserialization;
        }

        private static Position CreatePosition(FastFoodDbContext context, string position)
        {
            var newPosition = new Position()
            {
                Name = position
            };
            context.Positions.Add(newPosition);
            context.SaveChanges();

            return newPosition;
        }

        private static Category CreateCategory(FastFoodDbContext context, string category)
        {
            var currentCategory = new Category()
            {
                Name = category
            };
            context.Categories.Add(currentCategory);
            context.SaveChanges();

            return currentCategory;
        }


    }
}