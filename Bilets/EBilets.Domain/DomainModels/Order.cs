using EBilets.Domain.Identity;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

namespace EBilets.Domain.DomainModels
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }

        public EBiletsUser User { get; set; }

        public DateTime OrderDate { get; set; }
        public virtual ICollection<BilletInOrders> BilletsInOrder { get; set; }


    }
}
