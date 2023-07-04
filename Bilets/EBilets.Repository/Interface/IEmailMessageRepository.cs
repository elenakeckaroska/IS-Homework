using EBilets.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EBilets.Repository.Interface
{
    public interface IEmailMessageRepository
    {
        void Update(EmailMessage entity);
    }
}
