using Musaca.Models;
using Musaca.Services.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Musaca.Services
{
    public interface IReceiptService
    {
        void CreateReceipt(string userId, List<Order> orders);

        IQueryable<Receipt> GetAllReceiptsByUser(string userId);

        IQueryable<Receipt> GetAllReceipts();

        ReceiptDetailsDTO GetReceiptDetails(string id);
    }
}
