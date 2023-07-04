using EBilets.Domain.Identity;
using EBilets.Reposiotory;
using EBilets.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EBilets.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<EBiletsUser> entities;
        string errorMessage = string.Empty;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<EBiletsUser>();
        }
        public IEnumerable<EBiletsUser> GetAll()
        {
            return entities.AsEnumerable();
        }

        public EBiletsUser Get(string id)
        {
            return entities
                .Where(u  => u.Id == id)
                .Include(z => z.UserCart)
                .Include("UserCart.BilletInShoppingCart")
                .Include("UserCart.BilletInShoppingCart.CurrentBillet")
                .First();
        }
        public void Insert(EBiletsUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(EBiletsUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(EBiletsUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
    }
}
