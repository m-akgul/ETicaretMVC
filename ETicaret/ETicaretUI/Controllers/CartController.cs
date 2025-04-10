using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ETicaretDal.Abstract;
using ETicaretData.Entities;
using ETicaretData.Helpers;
using ETicaretData.Identity;
using ETicaretData.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ETicaretUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IOrderDal _orderDal;
        private readonly IProductDal _productDal;
        private readonly UserManager<AppUser> _userManager;

        public CartController(IOrderDal orderDal, IProductDal productDal, UserManager<AppUser> userManager)
        {
            _orderDal = orderDal;
            _productDal = productDal;
            _userManager = userManager;
        }



        // GET: /<controller>/
        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "Cart");
            if (cart == null)
            {
                return View();
            }
            ViewBag.Total = cart.Sum(x => x.Product.Price * x.Quantity).ToString("c");
            SessionHelper.Count = cart.Count;
            return View(cart);
        }


        public IActionResult Buy(int id)
        {
            if (SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "Cart") == null)
            {
                var cart = new List<CartItem>();
                cart.Add(new CartItem
                {
                    Product = _productDal.Get(id),
                    Quantity = 1
                });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "Cart", cart);
            }
            else
            {
                var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "Cart");

                int index = isExits(cart, id);
                if (index < 0)
                {
                    cart.Add(new CartItem
                    {
                        Product = _productDal.Get(id),
                        Quantity = 1
                    });
                }
                else
                {
                    cart[index].Quantity++;
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "Cart", cart);
            }
            var promoCode = HttpContext.Session.GetString("PromoCode");
            if (!string.IsNullOrEmpty(promoCode))
            {
                ApplyPromoCode(promoCode);
            }
            return RedirectToAction("Index");
        }
        private int isExits(List<CartItem> cart, int id)
        {
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

        [HttpGet]
        public async Task<IActionResult> CheckOut()
        {
            var Cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "Cart");
            if (Cart == null) // Sepet boşsa uyarı ver
            {
                TempData["UrunYok"] = "Sepetinizde ürün yok...";
                return RedirectToAction("Index");
            }
            if (User.Identity.Name != null)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {
                    ViewBag.Email = user.Email;
                    var addressList = new List<SelectListItem>
                    {
                        new SelectListItem { Value = "0", Text = "Select Address" }
                    };
                    if (!string.IsNullOrEmpty(user.AddressTitle1))
                    {
                        addressList.Add(new SelectListItem { Value = user.AddressTitle1, Text = user.AddressTitle1 });
                        ViewBag.AddressTitle1 = user.AddressTitle1;
                    }
                    if (!string.IsNullOrEmpty(user.AddressTitle2))
                    {
                        addressList.Add(new SelectListItem { Value = user.AddressTitle2, Text = user.AddressTitle2 });
                        ViewBag.AddressTitle2 = user.AddressTitle2;
                    }
                    ViewBag.AddressList = addressList;
                    ViewBag.Address1 = user.Address1;
                    ViewBag.Address2 = user.Address2;
                    ViewBag.City1 = user.City1;
                    ViewBag.City2 = user.City2;
                    return View(new ShippingDetails { UserName = user.UserName, Email = user.Email });
                }
            }
            return View(new ShippingDetails());
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(ShippingDetails details)
        {
            var user = await _userManager.FindByNameAsync(details.UserName);

            if (user != null && User.Identity.Name == null)
            {
                ModelState.AddModelError("", "Zaten böyle bir kullanıcı adı var!");
                return View(details);
            }
            if (user != null)
            {
                ViewBag.Email = user.Email;
                var addressList = new List<SelectListItem>
                    {
                        new SelectListItem { Value = "0", Text = "Select Address" }
                    };
                if (!string.IsNullOrEmpty(user.AddressTitle1))
                {
                    addressList.Add(new SelectListItem { Value = user.AddressTitle1, Text = user.AddressTitle1 });
                    ViewBag.AddressTitle1 = user.AddressTitle1;
                }
                if (!string.IsNullOrEmpty(user.AddressTitle2))
                {
                    addressList.Add(new SelectListItem { Value = user.AddressTitle2, Text = user.AddressTitle2 });
                    ViewBag.AddressTitle2 = user.AddressTitle2;
                }
                ViewBag.AddressList = addressList;
                ViewBag.Address1 = user.Address1;
                ViewBag.Address2 = user.Address2;
                ViewBag.City1 = user.City1;
                ViewBag.City2 = user.City2;
            }
            if (details.AddressTitle.Equals("0"))
            {
                ModelState.AddModelError("", "Lütfen adres seçiniz!");
                return View(details);
            }
            var Cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "Cart");
            if (ModelState.IsValid)
            {
                SaveOrder(Cart, details);
                Cart.Clear();
                if (Cart.Count == 0)
                {
                    Cart = null;
                    SessionHelper.Count = 0;
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "Cart", Cart);
                return RedirectToAction("Index");
            }
            else
            {
                return View(details);
            }

        }
        private void SaveOrder(List<CartItem>? Cart, ShippingDetails details)
        {
            var total = Cart.Sum(x => x.Product.Price * x.Quantity);
            var discount = HttpContext.Session.GetObjectFromJson<decimal?>("Discount") ?? 0;
            var discountedTotal = total - discount;
            var order = new Order
            {
                OrderNumber = Guid.NewGuid().ToString("N"),
                OrderDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                Total = discountedTotal,
                UserName = details.UserName,
                AddressTitle = details.AddressTitle,
                Address = details.Address,
                City = details.City,
                orderState = EnumOrderState.Waiting,
                OrderLines = new List<OrderLine>()
            };
            foreach (var item in Cart)
            {
                var orderLine = new OrderLine
                {
                    Quantity = item.Quantity,
                    Price = item.Product.Price * item.Quantity,
                    ProductId = item.Product.Id
                };
                order.OrderLines.Add(orderLine);
                var product = _productDal.Get(item.Product.Id);
                product.TotalSales += item.Quantity;
                _productDal.Update(product);
            }
            _orderDal.Add(order);
        }

        [HttpPost]
        public IActionResult ApplyPromoCode(string promoCode)
        {
            var Cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "Cart");
            if (Cart != null && promoCode == "ExampleCode")
            {
                var discount = Cart.Sum(x => x.Product.Price * x.Quantity) * 0.05m;
                HttpContext.Session.SetObjectAsJson("Discount", discount);
                HttpContext.Session.SetString("PromoCode", promoCode);
                TempData["PromoCodeMessage"] = "Promo code applied successfully! 5% discount has been applied.";
            }
            else
            {
                HttpContext.Session.SetObjectAsJson("Discount", 0);
                HttpContext.Session.Remove("PromoCode");
                TempData["PromoCodeMessage"] = "Invalid promo code.";
            }
            return RedirectToAction("CheckOut");
        }

        public IActionResult Remove(int id)
        {
            var Cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "Cart");
            if (Cart != null)
            {
                Cart.RemoveAll(item => item.Product.Id == id);

                if (Cart.Count == 0)
                {
                    Cart = null;
                    SessionHelper.Count = 0;
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "Cart", Cart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult IncreaseQuantity(int id)
        {
            var Cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "Cart");
            if (Cart != null)
            {
                var item = Cart.FirstOrDefault(i => i.Product.Id == id);
                if (item != null)
                {
                    item.Quantity++;
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "Cart", Cart);
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult DecreaseQuantity(int id)
        {
            var Cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "Cart");
            if (Cart != null)
            {
                var item = Cart.FirstOrDefault(i => i.Product.Id == id);
                if (item != null)
                {
                    if (item.Quantity > 1)
                    {
                        item.Quantity--;
                    }
                    else
                    {
                        Cart.Remove(item);
                    }
                    if (Cart.Count == 0)
                    {
                        Cart = null;
                        SessionHelper.Count = 0;
                    }
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "Cart", Cart);
                }
            }

            return RedirectToAction("Index");
        }
    }
}

