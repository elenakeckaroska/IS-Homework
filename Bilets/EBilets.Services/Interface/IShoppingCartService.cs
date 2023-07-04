using EBilets.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace EBilets.Services.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCartDto getShoppingCartInfo(string userId);
        bool deleteProductFromSoppingCart(string userId, Guid productId);
        bool order(string userId);
    }
}
