using EBilets.Domain.DomainModels;
using System.Collections.Generic;

namespace EBilets.Domain.DTO
{
    public class ShoppingCartDto
    {
        public List<BilletInShoppingCart> Billets { get; set; }

        public double TotalPrice { get; set; }
    }
}
