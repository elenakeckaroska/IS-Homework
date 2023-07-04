using EBilets.Domain.DomainModels;
using EBilets.Domain.DTO;
using EBilets.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EBilets.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<EBiletsUser> userManager;
        private readonly SignInManager<EBiletsUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public AccountController(UserManager<EBiletsUser> userManager, SignInManager<EBiletsUser> signInManager, RoleManager<IdentityRole> roleManager)
        {

            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public IActionResult Register()
        {
            UserRegistrationDto model = new UserRegistrationDto();
            return View(model);
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Register(UserRegistrationDto request)
        {
            if (ModelState.IsValid)
            {
                var userCheck = await userManager.FindByEmailAsync(request.Email);
                if (userCheck == null)
                {
                    var user = new EBiletsUser
                    {
                        FirstName = request.Name,
                        LastName = request.LastName,
                        UserName = request.Email,
                        NormalizedUserName = request.Email,
                        Email = request.Email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        PhoneNumber = request.PhoneNumber,
                        UserCart = new ShoppingCart()
                    };
                    var result = await userManager.CreateAsync(user, request.Password);
                    if (result.Succeeded)
                    {
                        await roleManager.CreateAsync(new IdentityRole("USER"));
                        await userManager.AddToRoleAsync(user, "User");
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        if (result.Errors.Count() > 0)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("message", error.Description);
                            }
                        }
                        return View(request);
                    }
                }
                else
                {
                    ModelState.AddModelError("message", "Email already exists.");
                    return View(request);
                }
            }
            return View(request);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            UserLoginDto model = new UserLoginDto();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError("message", "Email not confirmed yet");
                    return View(model);

                }
                if (await userManager.CheckPasswordAsync(user, model.Password) == false)
                {
                    ModelState.AddModelError("message", "Invalid credentials");
                    return View(model);

                }

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Billets");
                }
                else
                {
                    ModelState.AddModelError("message", "Invalid login attempt");
                    return View(model);
                }
            }
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult AddUserToRole()
        {
            AddToRoleModel model = new AddToRoleModel();
            model.Roles = new List<string>() { "Admin", "User" };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToRole(AddToRoleModel model)
        {
            var email = model.Email;
            EBiletsUser user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                //throw new HttpException(404, "There is no user with this email");
                return NotFound();
            }
            else
            {
                var roleExists = await roleManager.RoleExistsAsync(model.SelectedRole);
                if (!roleExists)
                {
                    var role = new IdentityRole(model.SelectedRole);
                    await roleManager.CreateAsync(role);
                }

                var userRoles = await userManager.GetRolesAsync(user);
                await userManager.RemoveFromRolesAsync(user, userRoles);

                await userManager.AddToRoleAsync(user, model.SelectedRole);

                //UserManager.AddToRoleAsync(user.Id, model.SelectedRole);
            }
            return RedirectToAction("Login");
        }
    }
}

