

using Musaca.Data;
using Musaca.Models;
using Musaca.Models.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Musaca.Services
{
    public class OrderService : IOrderService
    {

        private readonly MusacaDbContext context;

        public OrderService(MusacaDbContext context)
        {
            this.context = context;
        }

        public void ChangeOrdersStatusToCompleted(List<Order> orders)
        {
            foreach (var order in orders)
            {
                order.Status = OrderStatus.Completed;
            }

            context.SaveChanges();
        }

        public void CreateOrder(string productId, int quantity, string userId)
        {
            var order = new Order()
            {
                Quantity = quantity,
                CashierId = userId,
                Status = OrderStatus.Active,               
            };

            var productOrder = new ProductOrder()
            {
                OrderId = order.Id,
                ProductId = productId
            };

            context.Orders.Add(order);
            context.ProducsOrders.Add(productOrder);

            this.context.SaveChanges();

        }

        public List<Order> GetAllOrdersWithStatusActive(string userId)
        {
            return this.context.Orders.Where(x => x.Status.ToString() == "Active" && x.CashierId == userId).ToList();
        }

        public IQueryable<Order> GetOrderProductsByUser(string userId)
        {
            return this.context.Orders.Where(x => x.CashierId == userId);
        }


    }
}
