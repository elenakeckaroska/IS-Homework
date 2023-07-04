using EBilets.Domain.DomainModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace EBilets.Domain.DTO
{
    public class AddToShoppingCartDto
    {
        public Billet SelectedBillet { get; set; }
        public Guid SelectedBilletId { get; set; }

        [Display(Name ="Number of tickets")]
        public int SelectedQuantity { get; set; }
    }
}
