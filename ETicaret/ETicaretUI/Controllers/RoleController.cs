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


namespace ETicaretUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleController(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            if (_roleManager.Roles.ToList() == null)
            {
                return NotFound("Roller bulunamadı...");
            }
            return View(_roleManager.Roles.Where(i => i.Name != "Admin").ToList());
        }

        [HttpGet]
        public async Task<IActionResult> Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel role)
        {
            var _role = await _roleManager.FindByNameAsync(role.Name);
            if(_role == null)
            {
                var sonuc = await _roleManager.CreateAsync(new AppRole(role.Name));
                if (sonuc.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            
            return View(role);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            return View(role);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AppRole appRole)
        {
            var role = await _roleManager.FindByIdAsync(appRole.Id.ToString());
            role.Name = appRole.Name;
            role.NormalizedName = appRole.Name.ToUpper();
            var sonuc = await _roleManager.UpdateAsync(role);
            if (sonuc.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return View(appRole);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            var sonuc = await _roleManager.DeleteAsync(role);
            if (sonuc.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return NotFound("Silme İşlemi başarısız...");
        }
    }
}

