using EBilets.Domain.DomainModels;
using EBilets.Domain.DTO;
using EBilets.Repository.Interface;
using EBilets.Services.Interface;
using EBilets.Web.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace EBilets.Services.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<EmailMessage> _mailRepository;
        private readonly IRepository<BilletInOrders> _productInOrderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Billet> _productRepository;
        private readonly OrderCompletionNotifier _orderCompletionNotifier;



        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, IUserRepository userRepository, IRepository<Order> orderRepository, 
            IRepository<BilletInOrders> productInOrderRepository,IRepository<Billet> productRepository, IRepository<EmailMessage> mailRepository
           , OrderCompletionNotifier orderCompletionNotifier)

        {
            _shoppingCartRepository = shoppingCartRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _productInOrderRepository = productInOrderRepository;
            _mailRepository = mailRepository;
            _productRepository = productRepository;
            _orderCompletionNotifier = orderCompletionNotifier;
        }


        public bool deleteProductFromSoppingCart(string userId, Guid productId)
        {
            if (!string.IsNullOrEmpty(userId) && productId != null)
            {
                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                var itemToDelete = userShoppingCart.BilletInShoppingCart.Where(z => z.BilletId.Equals(productId)).FirstOrDefault();

                userShoppingCart.BilletInShoppingCart.Remove(itemToDelete);

                this._shoppingCartRepository.Update(userShoppingCart);

                return true;
            }
            return false;
        }

        public ShoppingCartDto getShoppingCartInfo(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var loggedInUser = this._userRepository.Get(userId);

                var userCard = loggedInUser.UserCart;

                var allProducts = userCard.BilletInShoppingCart.ToList();

                var allProductPrices = allProducts.Select(z => new
                {
                    ProductPrice = z.CurrentBillet.BilletPrice,
                    Quantity = z.Quantity
                }).ToList();

                double totalPrice = 0.0;

                foreach (var item in allProductPrices)
                {
                    totalPrice += item.Quantity * item.ProductPrice;
                }

                var reuslt = new ShoppingCartDto
                {
                    Billets = allProducts,
                    TotalPrice = totalPrice
                };

                return reuslt;
            }
            return new ShoppingCartDto();
        }

        public bool order(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var loggedInUser = this._userRepository.Get(userId);
                var userCard = loggedInUser.UserCart;

                EmailMessage mail = new EmailMessage();
                mail.MailTo = loggedInUser.Email;
                mail.Subject = "Sucessfuly created order!";
                mail.Status = false;


                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    User = loggedInUser,
                    UserId = userId,
                    OrderDate = DateTime.Now
                };

                this._orderRepository.Insert(order);

                List<BilletInOrders> productInOrders = new List<BilletInOrders>();

                var result = userCard.BilletInShoppingCart.Select(z => new BilletInOrders
                {
                    Id = Guid.NewGuid(),
                    BilletId = z.CurrentBillet.Id,
                    Billet = z.CurrentBillet,
                    OrderId = order.Id,
                    Order = order,
                    Quantity = z.Quantity
                }).ToList();

                StringBuilder sb = new StringBuilder();

                var totalPrice = 0.0;

                sb.AppendLine("Your order is completed. The order conatins: ");

                for (int i = 1; i <= result.Count(); i++)
                {
                    var currentItem = result[i - 1];
                    totalPrice += currentItem.Quantity * currentItem.Billet.BilletPrice;
                    sb.AppendLine(i.ToString() + ". Ticket/s for " + currentItem.Billet.BilletName + "movie for: " + currentItem.Quantity + "people with price of: $" + currentItem.Billet.BilletPrice);
                }

                sb.AppendLine("Total price for your order: " + totalPrice.ToString());

                mail.Content = sb.ToString();


                productInOrders.AddRange(result);

                foreach (var item in productInOrders)
                {
                    this._productInOrderRepository.Insert(item);
                    
                    var billet = this._productRepository.Get(item.Billet.Id);
                    billet.Quantity = billet.Quantity - item.Quantity;
                    this._productRepository.Update(billet);

                }

                loggedInUser.UserCart.BilletInShoppingCart.Clear();

                this._userRepository.Update(loggedInUser);
                this._mailRepository.Insert(mail);


                //mailNotifier
                _orderCompletionNotifier.NotifyOrderCompleted(order);




                return true;
            }          


            return false;
        }


    }
}
