using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ETicaretDal.Abstract;
using ETicaretDal.Concrete;
using ETicaretData.Context;
using ETicaretData.Entities;
using ETicaretData.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ETicaretUI.Controllers
{
    [Authorize(Roles = "Admin,Personnel")]
    public class OrderController : Controller
    {
        private readonly ETicaretContext _context;
        private readonly IOrderDal _orderDal;
        private readonly IProductDal _productDal;

        public OrderController(ETicaretContext context, IOrderDal orderDal, IProductDal productDal)
        {
            _context = context;
            _orderDal = orderDal;
            _productDal = productDal;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(_orderDal.GetAll());
        }

        [HttpGet]
        public async Task<IActionResult> Approve(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Home", "Error");
            }
            var order = _orderDal.Get(Convert.ToInt32(id));
            if (order == null)
            {
                return RedirectToAction("Home", "Error");
            }
            if (order.orderState == EnumOrderState.Canceled)
            {
                return RedirectToAction("Index");
            }
            return View(order);
        }
        [HttpPost]
        public async Task<IActionResult> Approve(bool isApproved, [Bind("Id", "OrderNumber", "OrderDate", "Total", "UserName", "AddressTitle", "Address", "City", "orderState")] Order order)
        {
            if (ModelState.IsValid)
            {
                if (isApproved)
                {
                    order.orderState = EnumOrderState.Approved;
                    order.ApprovedBy = User.Identity.Name;
                    _orderDal.Update(order);
                }
                else
                {
                    order.orderState = EnumOrderState.Waiting;
                    _orderDal.Update(order);
                }
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
            var order = _context.Orders
                .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return RedirectToAction("Home", "Error");
            }
            return View(order);
        }

        [AllowAnonymous]
        public IActionResult MyOrders(string? userName)
        {
            if (userName != null)
            {
                var orders = _orderDal.GetAll(p => p.UserName == userName);
                return View(orders);
            }
            else
            {
                return RedirectToAction("List", "Home");
            }
        }

        [AllowAnonymous]
        public ActionResult OrderDetails(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Home", "Error");
            }
            var order = _context.Orders
                .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return RedirectToAction("Home", "Error");
            }
            return View(order);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Cancel(int? id)
        {
            if (ModelState.IsValid)
            {
                var order = _orderDal.Get(o => o.Id == Convert.ToInt32(id), o => o.OrderLines);
                if (order != null && (order.orderState != EnumOrderState.Canceled || order.orderState != EnumOrderState.Approved))
                {
                    foreach (var orderLine in order.OrderLines)
                    {
                        var product = _productDal.Get(orderLine.ProductId);
                        product.TotalSales -= orderLine.Quantity;
                        _productDal.Update(product);
                    }
                    order.orderState = EnumOrderState.Canceled;
                    _orderDal.Update(order);
                    return RedirectToAction("MyOrders", "Order", new { userName = User.Identity.Name });
                }

            }
            return View();
        }
    }
}

