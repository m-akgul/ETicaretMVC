using ETicaretData.Helpers;
using ETicaretData.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretUI.ViewComponents.Default
{
    public class CartSummaryViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            var total = cart.Sum(x => x.Product.Price * x.Quantity);
            var discount = HttpContext.Session.GetObjectFromJson<decimal?>("Discount") ?? 0;
            var discountedTotal = total - discount;

            ViewBag.Total = total;
            ViewBag.DiscountedTotal = discountedTotal;
            ViewBag.Discount = discount;

            return View(cart);
        }
    }
}
