using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ETicaretDal.Abstract;
using ETicaretDal.Concrete;
using ETicaretData.Context;
using ETicaretData.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ETicaretUI.Controllers
{
    [Authorize(Roles = "Admin,Personnel")]
    public class CategoryController : Controller
    {
        private readonly ETicaretContext _context;
        private readonly ICategoryDal _categoryDal;

        public CategoryController(ETicaretContext context, ICategoryDal categoryDal)
        {
            _context = context;
            _categoryDal = categoryDal;
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            var categories = _categoryDal.GetAll(c => !c.IsDeleted, c => c.Products);
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id", "Name", "Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryDal.Add(category);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Home", "Error");
            }
            var category = _categoryDal.Get(Convert.ToInt32(id));
            if (category == null)
            {
                return RedirectToAction("Home", "Error");
            }
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Id", "Name", "Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryDal.Update(category);
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
            var category = _categoryDal.Get(Convert.ToInt32(id));
            if (category == null)
            {
                return RedirectToAction("Home", "Error");
            }
            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return RedirectToAction("Home", "Error");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return RedirectToAction("Home", "Error");
            }
            if (ModelState.IsValid)
            {
                category.IsDeleted = true;
                _categoryDal.Delete(category);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

