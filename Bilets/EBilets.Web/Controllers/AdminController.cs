using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using EBilets.Domain.DomainModels;
using EBilets.Domain.DTO;
using EBilets.Domain.Identity;
using EBilets.Services.Implementation;
using EBilets.Services.Interface;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EBilets.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<EBiletsUser> _userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IBilletService _billetService;
        private readonly IUserService _userService;



        public AdminController(IOrderService orderService, UserManager<EBiletsUser> userManager,
            RoleManager<IdentityRole> roleManager, IBilletService billetService, IUserService userService)
        {
            this._orderService = orderService;
            this._userManager = userManager;
            this.roleManager = roleManager;
            this._billetService = billetService;
            this._userService = userService;
        }
        

        

        [HttpPost]
        public async Task<IActionResult> ReadingFromFileUsers(IFormFile file)
        {
            string fileName = file.FileName;
            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";

            using (FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            List<UserFromFileDto> users = _userService.ReadUsersFromExcelFile(fileName);

            foreach (var item in users)
            {
                var userCheck = await _userManager.FindByEmailAsync(item.Email);
                if (userCheck == null)
                {
                    var user = new EBiletsUser
                    {
                        FirstName = item.Email,
                        LastName = item.Email,
                        UserName = item.Email,
                        NormalizedUserName = item.Email,
                        Email = item.Email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        PhoneNumber = "075301213",
                        UserCart = new ShoppingCart()
                    };

                    var result = await _userManager.CreateAsync(user, item.Password);
                    if (result.Succeeded)
                    {
                        await roleManager.CreateAsync(new IdentityRole(item.Role.ToUpper()));
                        await _userManager.AddToRoleAsync(user, item.Role);
                        return RedirectToAction("Index", "Billets");
                    }
                }
            }

            return RedirectToAction("Index", "Billets");
        }

    }
}