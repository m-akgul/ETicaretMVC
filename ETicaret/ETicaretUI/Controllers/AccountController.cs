using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETicaretData.Identity;
using ETicaretData.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ETicaretUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.Name != null)
            {
                return RedirectToAction("List", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            var User = await _userManager.FindByNameAsync(login.UserName);
            if (User != null)
            {
                var sonuc = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, login.RememberMe, true);
                if (sonuc.Succeeded)
                {
                    return RedirectToAction("List", "Home");
                }
            }
            return View(login);
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.Name != null)
            {
                return RedirectToAction("List", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new AppUser
            {
                Name = model.Name,
                Surname = model.Surname,
                UserName = model.UserName,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var role = await _userManager.AddToRoleAsync(user, "Benutzer");
                if (role.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var error in role.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("List", "Home");
        }

        public async Task<IActionResult> Profile(string? userName)
        {
            if (User.Identity.Name == null)
            {
                return RedirectToAction("List", "Home");
            }
            if (userName == null)
            {
                return RedirectToAction("Home", "Error");
            }
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                return View(user);
            }

            return RedirectToAction("List", "Home");
        }

        public async Task<IActionResult> Edit(string? userName)
        {
            if (User.Identity.Name == null)
            {
                return RedirectToAction("List", "Home");
            }
            if (userName == null)
            {
                return RedirectToAction("Home", "Error");
            }
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                return View(user);
            }
            return RedirectToAction("List", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AppUser appUser)
        {
            if (User.Identity.Name == null)
            {
                return RedirectToAction("List", "Home");
            }
            if (appUser == null)
            {
                return RedirectToAction("Home", "Error");
            }
            var user = await _userManager.FindByNameAsync(appUser.UserName);
            if (user != null)
            {
                user.Name = appUser.Name;
                user.Surname = appUser.Surname;
                user.AddressTitle1 = appUser.AddressTitle1;
                user.Address1 = appUser.Address1;
                user.City1 = appUser.City1;
                user.AddressTitle2 = appUser.AddressTitle2;
                user.Address2 = appUser.Address2;
                user.City2 = appUser.City2;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Profile", new { userName = appUser.UserName });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(appUser);
                }
            }
            return RedirectToAction("List", "Home");
        }
    }
}

