using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_253502_POBORTSEVA.Domain.Entities;
using WEB_253502_POBORTSEVA.UI.Services.ProductService;

namespace WEB_253502_POBORTSEVA.UI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private IProductService _productService;
        private Cart _cart;

        public CartController(IProductService productService, Cart cart)
        {
            _productService = productService;
            _cart = cart;
        }

        public IActionResult Index()
        {
            return View(_cart);
        }

        [Route("[controller]/add/{id:int}")]
        public async Task<ActionResult> Add(int id, string returnUrl)
        {
            var data = await _productService.GetProductByIdAsync(id);
            if (data.Successfull)
            {
                _cart.AddToCart(data.Data!);
            }

            return Redirect(returnUrl);
        }

        [Route("[controller]/remove/{id:int}")]
        public IActionResult RemoveItem(int id, string redirectUrl)
        {
            _cart.RemoveItems(id);
            return Redirect(redirectUrl);
        }

        [Route("[controller]/clear")]
        public IActionResult Clear()
        {
            _cart.ClearAll();
            return RedirectToAction("Index");
        }
    }
}
