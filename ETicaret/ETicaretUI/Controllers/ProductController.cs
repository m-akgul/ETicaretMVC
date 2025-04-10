using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETicaretDal.Abstract;
using ETicaretData.Context;
using ETicaretData.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ETicaretUI.Controllers
{
    [Authorize(Roles = "Admin,Personnel")]
    public class ProductController : Controller
    {
        private readonly ETicaretContext _context;
        private readonly IProductDal _productDal;
        private readonly ICategoryDal _categoryDal;

        public ProductController(ETicaretContext context, IProductDal productDal, ICategoryDal categoryDal)
        {
            _context = context;
            _productDal = productDal;
            _categoryDal = categoryDal;
        }

        public async Task<IActionResult> Index()
        {
            return View(_productDal.GetAll(p => !p.IsDeleted, p => p.Category, p => p.Supplier));
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "Id", "CompanyName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id", "Name", "Image", "Stock", "Price", "IsHome", "IsApproved", "CategoryId", "SupplierID")] Product product)
        {
            if (ModelState.IsValid)
            {
                _productDal.Add(product);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "Id", "CompanyName", product.SupplierID);
            return View();
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return RedirectToAction("Home", "Error");
            }
            var product = _productDal.Get(p => p.Id == Convert.ToInt32(id), p => p.Category, p => p.Supplier);

            if (product == null)
            {
                return RedirectToAction("Home", "Error");
            }
            return View(product);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return RedirectToAction("Home", "Error");
            }
            var product = _productDal.Get(Convert.ToInt32(id));
            if (product == null)
            {
                return RedirectToAction("Home", "Error");
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "Id", "CompanyName");
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Id", "Name", "Description", "Image", "Stock", "Price", "IsHome", "IsApproved", "CategoryId", "SupplierID")] Product product)
        {
            if (ModelState.IsValid)
            {
                _productDal.Update(product);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "Id", "CompanyName", product.SupplierID);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return RedirectToAction("Home", "Error");
            }
            var product = _productDal.Get(p => p.Id == id, p => p.Category);

            if (product == null)
            {
                return RedirectToAction("Home", "Error");
            }
            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return RedirectToAction("Home", "Error");
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return RedirectToAction("Home", "Error");
            }
            if (ModelState.IsValid)
            {
                product.IsDeleted = true;
                _productDal.Delete(product);
            }
            return RedirectToAction(nameof(Index));
        }

    }
}

