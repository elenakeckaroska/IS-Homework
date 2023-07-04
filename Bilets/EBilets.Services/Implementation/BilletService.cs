using ClosedXML.Excel;
using EBilets.Domain.DomainModels;
using EBilets.Domain.DTO;
using EBilets.Repository.Interface;
using EBilets.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EBilets.Services.Implementation
{
    public class BilletService : IBilletService
    {
        private readonly IRepository<Billet> _productRepository;
        private readonly IRepository<BilletInShoppingCart> _productInShoppingCartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<BilletInOrders> _ordersRepository;

        public BilletService(IRepository<Billet> productRepository, IRepository<BilletInShoppingCart> productInShoppingCartRepository, 
            IUserRepository userRepository, IRepository<BilletInOrders> ordersRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
            _productInShoppingCartRepository = productInShoppingCartRepository;
            _ordersRepository = ordersRepository;
        }


        public bool AddToShoppingCart(AddToShoppingCartDto item, string userID)
        {
            var user = this._userRepository.Get(userID);

            var userShoppingCard = user.UserCart;

            if (item.SelectedBilletId != null && userShoppingCard != null)
            {
                var billet = this.GetDetailsForBillet(item.SelectedBilletId);
                //{896c1325-a1bb-4595-92d8-08da077402fc}

                if (billet != null)
                {
                    BilletInShoppingCart itemToAdd = new BilletInShoppingCart
                    {
                        Id = Guid.NewGuid(),
                        CurrentBillet = billet,
                        BilletId = billet.Id,
                        UserCart = userShoppingCard,
                        ShoppingCartId = userShoppingCard.Id,
                        Quantity = item.SelectedQuantity
                    };

                    var existing = userShoppingCard.BilletInShoppingCart
                        .Where(z => z.ShoppingCartId == userShoppingCard.Id && z.BilletId == itemToAdd.BilletId).FirstOrDefault();

                    if (existing != null)
                    {
                        existing.Quantity += itemToAdd.Quantity;
                        this._productInShoppingCartRepository.Update(existing);

                    }
                    else
                    {
                        this._productInShoppingCartRepository.Insert(itemToAdd);
                    }

                    

                    return true;
                }
                return false;
            }
            return false;
        }

        public void CreateNewProduct(Billet b)
        {
            this._productRepository.Insert(b); 

        }

        public void DeleteBillet(Guid id)

        {
            var b = this.GetDetailsForBillet(id);

            this._productRepository.Delete(b);    
        }

        public List<Billet> GetAllBillets()
        {

            return this._productRepository.GetAll().ToList();
        }

        public Billet GetDetailsForBillet(Guid? id)
        {
            return this._productRepository.Get(id);

        }

        public AddToShoppingCartDto GetShoppingCartInfo(Guid? id)
        {
            var product = this.GetDetailsForBillet(id);
            AddToShoppingCartDto model = new AddToShoppingCartDto
            {
                SelectedBillet = product,
                SelectedBilletId = product.Id,
                SelectedQuantity = 1
            };

            return model;
        }

        public void UpdeteExistingProduct(Billet b)
        {
            this._productRepository.Update(b);

        }

        public List<string> allGenres()
        {
            return  this.GetAllBillets().Select(b => b.Genre).Distinct().ToList();
        }

        public byte[] ExportBilletsByGenreToExcel(string selectedGenre)
        {
            List<Billet> billets1 = new List<Billet>();
            foreach(var o in _ordersRepository.GetAll())
            {
                Billet billet = _productRepository.Get(o.BilletId);
                billets1.Add(billet);
            }

            var billets = GetAllBillets()
                .Where(b => b.Genre == selectedGenre && billets1.Contains(b))
                .ToArray();

            using (var workBook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workBook.Worksheets.Add("All Billets");
                worksheet.Cell(1, 2).Value = "Genre: ";
                worksheet.Cell(1, 3).Value = selectedGenre;

                worksheet.Cell(2, 1).Value = "Movie Title";
                worksheet.Cell(2, 2).Value = "Ticket Price";
                worksheet.Cell(2, 3).Value = "Projection Date";
                worksheet.Cell(2, 4).Value = "Available tickets";

                for (int i = 2; i <= billets.Length + 1; i++)
                {
                    var item = billets[i - 2];

                    worksheet.Cell(i + 1, 1).Value = item.BilletName;
                    worksheet.Cell(i + 1, 2).Value = item.BilletPrice;
                    worksheet.Cell(i + 1, 3).Value = item.ValidDate.ToString();
                    worksheet.Cell(i + 1, 4).Value = item.Quantity;
                }

                using (var stream = new MemoryStream())
                {
                    workBook.SaveAs(stream);

                    return stream.ToArray();
                }
            }
        }

    }
}
