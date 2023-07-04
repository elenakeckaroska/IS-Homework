using EBilets.Domain.DomainModels;
using EBilets.Repository.Interface;
using EBilets.Services.Interface;
using GemBox.Document;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Text;

namespace EBilets.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        }

        public MemoryStream CreateInvoice(Guid id)
        {
            var result = this._orderRepository.getOrderDetails(id);

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");

            var document = DocumentModel.Load(templatePath);

            document.Content.Replace("{{OrderNumber}}", result.Id.ToString());
            document.Content.Replace("{{UserName}}", result.User.Email);


            StringBuilder sb = new StringBuilder();

            var total = 0.0;

            foreach (var item in result.BilletsInOrder)
            {
                total += item.Quantity * item.Billet.BilletPrice;
                sb.AppendLine(item.Billet.BilletName + " with quantity of: " + item.Quantity + " and price of: $" + item.Billet.BilletPrice);
            }

            document.Content.Replace("{{ProductList}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", "$" + total.ToString());

            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());

            return stream;
        }

        public List<Order> getAllOrders()
        {
            return this._orderRepository.getAllOrders();
        }

        public Order getOrderDetails(Guid id)
        {
            return this._orderRepository.getOrderDetails(id);
        }

        public List<Order> getOrdersForUser(string userId)
        {
            return _orderRepository.getOrdersForUser(userId);
        }
    }
}
