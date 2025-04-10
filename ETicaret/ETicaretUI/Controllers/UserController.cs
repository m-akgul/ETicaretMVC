using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ETicaretData.Identity;
using ETicaretData.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ETicaretUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var adminIds = admins.Select(a => a.Id).ToList();
            var users = new List<AppUser>();
            if (adminIds is null)
            {
                users = _userManager.Users.ToList();
            }
            else
            {
                users = _userManager.Users.Where(i => !adminIds.Contains(i.Id)).ToList();
            }
            var userList = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new UserRoleViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Username = user.UserName,
                    Roles = roles.ToList()
                });
            }
            return View(userList);
        }

        [HttpGet]
        public async Task<IActionResult> RoleAssign(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var roles = _roleManager.Roles.Where(i => i.Name != "Admin").ToList();
            var userRoles = await _userManager.GetRolesAsync(user);
            var RoleAssigns = new List<RoleAssignModels>();
            roles.ForEach(role => RoleAssigns.Add(new RoleAssignModels
            {
                HasAssigned = userRoles.Contains(role.Name),
                Id = role.Id,
                Name = role.Name
            }));
            ViewBag.name = user.Name;
            return View(RoleAssigns);
        }
        [HttpPost]
        public async Task<IActionResult> RoleAssign(List<RoleAssignModels> models, int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            foreach (var role in models)
            {
                if (role.HasAssigned)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var sonuc = await _userManager.DeleteAsync(user);
            if (sonuc.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return NotFound("Silme İşlemi başarısız...");
        }
    }
}

