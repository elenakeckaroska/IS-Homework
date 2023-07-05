using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.VisualBasic;
using System.Collections.Generic;

namespace EBilets.Domain.DomainModels
{
    public class Billet : BaseEntity
    {
       
        [Required]
        [Display(Name ="Movie Title")]
        public string BilletName { get; set; }
        [Required]
        [Display(Name = "Poster")]

        public string BilletImage { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        [Display(Name = "Movie Description")]

        public string BilletDescription { get; set; }
        [Required]
        [Display(Name = "Ticket Price")]

        public double BilletPrice { get; set; }
        [Required]
        [Display(Name = "Date and Time")]

        public DateTime ValidDate { get; set; }

        [Required]
        [Display(Name = "Available Tickets")]

        public int Quantity { get; set; }

        public virtual ICollection<BilletInShoppingCart> BilletInShoppingCart { get; set; }
        public virtual ICollection<BilletInOrders> BilletInOrders { get; set; }



    }
}
