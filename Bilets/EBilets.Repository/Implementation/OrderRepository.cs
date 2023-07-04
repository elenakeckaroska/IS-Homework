using EBilets.Domain.DomainModels;
using EBilets.Reposiotory;
using EBilets.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace EBilets.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Order> entities;
        string errorMessage = string.Empty;

        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Order>();
        }
        public List<Order> getOrdersForUser(string userId)
        {
            return entities
                .Where(z => z.UserId == userId)
                .Include(z => z.BilletsInOrder)
                .Include("BilletsInOrder.Billet")
                .ToListAsync().Result;
        }

        public Order getOrderDetails(Guid id)
        {
            return entities
               .Include(z => z.User)
               .Include(z => z.BilletsInOrder)
               .Include("BilletsInOrder.Billet")
               .SingleOrDefaultAsync(z => z.Id == id).Result;
        }

        public List<Order> getAllOrders()
        {
            return entities
                           .Include(z => z.BilletsInOrder)
                           .Include("BilletsInOrder.Billet")
                           .ToListAsync().Result;
        }
    }
}
