using EBilets.Domain.DomainModels;
using System;

namespace EBilets.Domain.DomainModels
{
    public class BilletInShoppingCart : BaseEntity
    {
        public Guid BilletId { get; set; }
        public virtual Billet CurrentBillet { get; set; }

        public Guid ShoppingCartId { get; set; }
        public virtual ShoppingCart UserCart { get; set; }

        public int Quantity { get; set; }
    }
}
