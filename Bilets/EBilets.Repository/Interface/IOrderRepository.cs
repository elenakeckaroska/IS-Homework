using EBilets.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EBilets.Repository.Interface
{
    public interface IOrderRepository
    {
        public List<Order> getAllOrders();
        public Order getOrderDetails(Guid id);
        public List<Order> getOrdersForUser(string userId);


    }
}
