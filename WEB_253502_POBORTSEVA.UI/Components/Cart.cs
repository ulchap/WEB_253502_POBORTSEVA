using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_253502_POBORTSEVA.Domain.Entities;
using WEB_253502_POBORTSEVA.UI.Models;
using WEB_253502_POBORTSEVA.UI.Extensions;

namespace WEB_253502_POBORTSEVA.UI.Components
{
    public class CartViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            Cart cartInfo = HttpContext.Session.Get<Cart>("cart") ?? new();
            return View(cartInfo);
        }
    }
}
