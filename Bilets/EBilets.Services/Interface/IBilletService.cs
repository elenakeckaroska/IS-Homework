using EBilets.Domain.DomainModels;
using EBilets.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace EBilets.Services.Interface
{
    public interface IBilletService
    {
        List<Billet> GetAllBillets();
        Billet GetDetailsForBillet(Guid? id);
        void CreateNewProduct(Billet b);
        void UpdeteExistingProduct(Billet b);
        AddToShoppingCartDto GetShoppingCartInfo(Guid? id);
        void DeleteBillet(Guid id);
        bool AddToShoppingCart(AddToShoppingCartDto item, string userID);

        public List<string> allGenres();

        public byte[] ExportBilletsByGenreToExcel(string selectedGenre);


    }
}
