using EBilets.Domain.Identity;
using System;
using System.Collections.Generic;

namespace EBilets.Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
        public string OwnerId { get; set; }
        public virtual EBiletsUser Owner { get; set; }

        public virtual ICollection<BilletInShoppingCart> BilletInShoppingCart { get; set; }

    }
}
