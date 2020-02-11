using Musaca.Models;
using System.Collections.Generic;
using System.Linq;

namespace Musaca.Services
{
    public interface IOrderService
    {
        void CreateOrder(string productId, int Quantity, string userId);

        IQueryable<Order> GetOrderProductsByUser(string userId);

        List<Order> GetAllOrdersWithStatusActive(string userId);

        void ChangeOrdersStatusToCompleted(List<Order> orders);
    }
}
