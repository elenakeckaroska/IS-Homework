using EBilets.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EBilets.Repository.Interface
{
    
        public interface IUserRepository
        {
            IEnumerable<EBiletsUser> GetAll();
            EBiletsUser Get(string id);
            void Insert(EBiletsUser entity);
            void Update(EBiletsUser entity);
            void Delete(EBiletsUser entity);
        }
    
}
