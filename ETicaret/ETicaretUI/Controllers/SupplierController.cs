using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ETicaretDal.Abstract;
using ETicaretDal.Concrete;
using ETicaretData.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ETicaretUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SupplierController : Controller
	{
		private readonly ISupplierDal _supplierDal;

        public SupplierController(ISupplierDal supplierDal)
        {
            _supplierDal = supplierDal;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
			return View(_supplierDal.GetAll(p => !p.IsDeleted));
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add([Bind("Id", "CompanyName", "Address", "City", "PhoneNumber")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _supplierDal.Add(supplier);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Home", "Error");
            }
            var supplier = _supplierDal.Get(Convert.ToInt32(id));

            if (supplier == null)
            {
                return RedirectToAction("Home", "Error");
            }
            return View(supplier);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Home", "Error");
            }
            var supplier = _supplierDal.Get(Convert.ToInt32(id));
            if (supplier == null)
            {
                return RedirectToAction("Home", "Error");
            }
            return View(supplier);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Id", "CompanyName", "Address", "City", "PhoneNumber")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _supplierDal.Update(supplier);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Home", "Error");
            }
            var supplier = _supplierDal.Get(Convert.ToInt32(id));

            if (supplier == null)
            {
                return RedirectToAction("Home", "Error");
            }
            return View(supplier);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplier = _supplierDal.Get(Convert.ToInt32(id));
            if (supplier == null)
            {
                return RedirectToAction("Home", "Error");
            }
            if (ModelState.IsValid)
            {
                supplier.IsDeleted = true;
                _supplierDal.Delete(supplier);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

