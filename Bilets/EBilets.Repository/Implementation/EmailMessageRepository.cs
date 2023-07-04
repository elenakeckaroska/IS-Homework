using EBilets.Domain.DomainModels;
using EBilets.Reposiotory;
using EBilets.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EBilets.Repository.Implementation
{
    public class EmailMessageRepository : IEmailMessageRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<EmailMessage> entities;
        string errorMessage = string.Empty;

        public EmailMessageRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<EmailMessage>();
        }
        public void Update(EmailMessage entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }
    }
}
