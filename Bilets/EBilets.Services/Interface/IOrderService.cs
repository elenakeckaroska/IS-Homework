using EBilets.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Text;

namespace EBilets.Services.Interface
{
    public interface IOrderService
    {
        public List<Order> getAllOrders();
        public Order getOrderDetails(Guid id);

        public List<Order> getOrdersForUser(string userId);

        public MemoryStream CreateInvoice(Guid id);

    }
}
