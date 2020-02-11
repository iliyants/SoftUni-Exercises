using Microsoft.EntityFrameworkCore;
using Musaca.Data;
using Musaca.Models;
using Musaca.Services.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Musaca.Services
{
    public class ReceiptService : IReceiptService
    {

        private readonly MusacaDbContext context;


        public ReceiptService(MusacaDbContext context)
        {
            this.context = context;
        }

        public void CreateReceipt(string userId, List<Order> orders)
        {
            var receipt = new Receipt()
            {
                IssuedOn = DateTime.UtcNow,
                CashierId = userId,
            };

            foreach (var order in orders)
            {
                var receiptOrder = new ReceiptOrder()
                {
                    OrderId = order.Id,
                    ReceiptId = receipt.Id
                };
                context.ReceiptsOrders.Add(receiptOrder);
            }

            context.Receipts.Add(receipt);

            context.SaveChanges();
        }

        public IQueryable<Receipt> GetAllReceipts()
        {
            return this.context.Receipts;
        }

        public IQueryable<Receipt> GetAllReceiptsByUser(string userId)
        {
            return this.context.Receipts.Where(x => x.CashierId == userId);
        }

        public ReceiptDetailsDTO GetReceiptDetails(string id)
        {
            return this.context.Receipts
                .Where(x => x.Id == id)
                .Select(x => new ReceiptDetailsDTO()
                {
                    Id = x.Id,
                    Cashier = x.Cashier.Username,
                    IssuedOn = x.IssuedOn,
                    Products = x.ReceiptOrders.Select(p => new ProductView()
                    {
                        Name = p.Order.OrderProducts.Select(n => n.Product.Name).SingleOrDefault(),
                        Quantity = p.Order.Quantity,
                        Price = $"{p.Order.Quantity * p.Order.OrderProducts.Select(z => z.Product.Price).SingleOrDefault():F2}"
                    }).ToList(),
                    Total = $"{x.ReceiptOrders.Select(o => o.Order.Quantity * o.Order.OrderProducts.Sum(p => p.Product.Price)).Sum():F2}"
                }).SingleOrDefault();

        }
    }
}
