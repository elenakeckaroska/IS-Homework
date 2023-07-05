
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EBilets.Reposiotory;
using EBilets.Domain.Identity;
using EBilets.Domain.DTO;
using EBilets.Domain.DomainModels;
using EBilets.Services.Interface;

namespace EBilets.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }


        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(this._shoppingCartService.getShoppingCartInfo(userId));
        }

        public IActionResult DeleteFromShoppingCart(Guid billetId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._shoppingCartService.deleteProductFromSoppingCart(userId, billetId);

            if (result)
            {
                return RedirectToAction("Index", "ShoppingCart");
            }
            else
            {
                return RedirectToAction("Index", "ShoppingCart");
            }
        }

        public IActionResult OrderNow()
        {
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._shoppingCartService.order(userId);

            return RedirectToAction("Checkout");
        }

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Payment()
        {
            return RedirectToAction("Index","Order");
        }

    }


}

