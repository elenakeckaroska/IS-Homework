using EBilets.Domain.DomainModels;
using EBilets.Domain.Identity;
using EBilets.Reposiotory;
using EBilets.Services.Interface;
using GemBox.Document;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EBilets.Web.Controllers
{
    public class OrderController : Controller
    {   
        private readonly IOrderService _orderService;


        public OrderController(IOrderService orderService)
        {
            this._orderService = orderService;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Order> allOrders = this._orderService.getOrdersForUser(userId);

            return View(allOrders);
        }

        public IActionResult CreateInvoice(Guid id)
        {

            var stream = _orderService.CreateInvoice(id);

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportOrderInvoice.pdf");


        }
    }
}
