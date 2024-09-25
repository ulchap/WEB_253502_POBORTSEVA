using Microsoft.AspNetCore.Mvc;
using WEB_253502_POBORTSEVA.UI.Models;

namespace WEB_253502_POBORTSEVA.UI.Components
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = new CartViewModel
            {
                TotalPrice = 0.0m,
                Count = 0,
            };

            return View(cart);

        }
    }
}
