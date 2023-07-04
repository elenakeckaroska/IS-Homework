using System;
using EBilets.Domain.DomainModels;

namespace EBilets.Domain.DomainModels
{
    public class BilletInOrders : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Guid BilletId { get; set; }
        public Billet Billet { get; set; }
        public int Quantity { get; set; }

    }
}
