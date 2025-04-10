using System.Diagnostics;
using ETicaretDal.Abstract;
using ETicaretData.ViewModels;
using ETicaretUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ETicaretUI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICategoryDal _categoryDal;
    private readonly IProductDal _productDal;

    public HomeController(ILogger<HomeController> logger, ICategoryDal categoryDal, IProductDal productDal)
    {
        _logger = logger;
        _categoryDal = categoryDal;
        _productDal = productDal;
    }

    public IActionResult List(int? Id, string sortOrder)
    {
        ViewBag.Id = Id;
        ViewBag.SortOrder = sortOrder;
        var product = _productDal.GetAll(p => p.IsApproved && !p.IsDeleted);
        if (Id != null)
        {
            product = product.Where(p => p.CategoryId == Id).ToList();
        }
        switch (sortOrder)
        {
            case "mostSellers":
                product = product.OrderByDescending(p => p.TotalSales).ToList();
                break;
            case "priceAsc":
                product = product.OrderBy(p => p.Price).ToList();
                break;
            case "priceDesc":
                product = product.OrderByDescending(p => p.Price).ToList();
                break;
            default:
                break;
        }
        var models = new ListViewModel()
        {
            Categories = _categoryDal.GetAll(),
            Products = product
        };
        return View(models);
    }

    [HttpGet]
    public ActionResult Details(int? id)
    {
        if (id == null || _productDal.Get(Convert.ToInt32(id)) == null)
        {
            return RedirectToAction("Home", "Error");
        }
        var product = _productDal.Get(Convert.ToInt32(id));
        if (product == null)
        {
            return RedirectToAction("Home", "Error");
        }
        return View(product);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

