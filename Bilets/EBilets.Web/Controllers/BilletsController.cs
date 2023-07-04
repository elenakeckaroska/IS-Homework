using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using System.Buffers.Text;
using System.Data.SqlTypes;
using System.Reflection;
using EBilets.Domain;
using EBilets.Reposiotory;
using EBilets.Domain.DTO;
using EBilets.Services.Interface;
using System.Security.Cryptography;
using EBilets.Domain.DomainModels;
using System.IO;
using ClosedXML.Excel;

namespace EBilets.Web.Controllers
{
    public class BilletsController : Controller
    {
        private readonly IBilletService _billetService;


        public BilletsController(IBilletService billetService)
        {
            _billetService = billetService;
        }
        

        // GET: Billets
        public IActionResult Index(DateTime? startDate, DateTime? endDate)
        {
            BilletsByGenreDto billetsByGenre = new BilletsByGenreDto();
            billetsByGenre.Genres = this._billetService.allGenres();
            billetsByGenre.Billets = this._billetService.GetAllBillets();
            billetsByGenre.SelectedGenre = this._billetService.allGenres().First();

            if (startDate.HasValue)
            {
                billetsByGenre.Billets = billetsByGenre.Billets.Where(b => b.ValidDate >= startDate.Value.Date).ToList();
            }

            if (endDate.HasValue)
            {
                billetsByGenre.Billets = billetsByGenre.Billets.Where(b => b.ValidDate <= endDate.Value.Date).ToList();
            }

            return View(billetsByGenre);
        }

        [HttpPost]

        public IActionResult ExportBilletsByGenre(BilletsByGenreDto billetsByGenreDto)
        {
            var selectedGenre = billetsByGenreDto.SelectedGenre;
            var fileContent = _billetService.ExportBilletsByGenreToExcel(selectedGenre);

            string fileName = "Billets.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return File(fileContent, contentType, fileName);
        }

        // GET: Billets/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billet = this._billetService.GetDetailsForBillet(id);
            //var product = await _context.Products
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (billet == null)
            {
                return NotFound();
            }

            return View(billet);
        }

        // GET: Billets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Billets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,BilletName,BilletImage,BilletDescription,BilletPrice,ValidDate,Quantity, Genre")] Billet billet)
        {
            if (ModelState.IsValid)
            {

                this._billetService.CreateNewProduct(billet);
                return RedirectToAction(nameof(Index));
            }
            return View(billet);
        }

        // GET: Billets/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billet = this._billetService.GetDetailsForBillet(id);
            if (billet == null)
            {
                return NotFound();
            }
            return View(billet);
        }

        // POST: Billets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,BilletName,BilletImage,BilletDescription,BilletPrice,ValidDate,Quantity,Genre")] Billet billet)
        {
            if (id != billet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this._billetService.UpdeteExistingProduct(billet);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BilletExists(billet.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(billet);
        }

        // GET: Billets/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = this._billetService.GetDetailsForBillet(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Billets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            this._billetService.GetDetailsForBillet(id);
            return RedirectToAction(nameof(Index));
        }

        private bool BilletExists(Guid id)
        {
            return _billetService.GetDetailsForBillet(id) != null;
        }

        public IActionResult AddBilletToCart(Guid id)
        {
            var model = this._billetService.GetShoppingCartInfo(id);

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBilletToCart(AddToShoppingCartDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var userShoppingCard = await _context.ShoppingCarts.Where(z => z.OwnerId.Equals(userId)).FirstOrDefaultAsync();
            var result = this._billetService.AddToShoppingCart(model, userId);

            if (result)
            {
                return RedirectToAction("Index", "ShoppingCart");
            }
            return View(model);


        }

        
    }

    







}
