using Microsoft.AspNetCore.Mvc;
using WEB_253502_POBORTSEVA.UI.Extensions;
using WEB_253502_POBORTSEVA.Domain.Entities;
using WEB_253502_POBORTSEVA.UI.Services.CategoryService;
using WEB_253502_POBORTSEVA.UI.Services.ProductService;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WEB_253502_POBORTSEVA.UI.Controllers
{
    [Route("catalog")]
    public class ProductController : Controller
    {
        IProductService _productService;
        ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [Route("{category?}")]
        public async Task<IActionResult> Index(string? category, int pageNo = 1)
        {

            var categoryResponse = await _categoryService.GetCategoryListAsync();
            if (!categoryResponse.Successfull)
                return NotFound(categoryResponse.ErrorMessage);

            var currentCategory = categoryResponse?.Data?.FirstOrDefault(c => c.NormalizedName == category)?.Name ?? "Все";

            ViewData["categories"] = categoryResponse.Data;
            ViewData["currentCategory"] = currentCategory;

            var productResponse = await _productService.GetProductListAsync(category, pageNo);
            if (!productResponse.Successfull)
                return NotFound(productResponse.ErrorMessage);

            var data = productResponse.Data;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_CatalogPartial", data);
            }
            
            return View(data);
        }
    }
}
