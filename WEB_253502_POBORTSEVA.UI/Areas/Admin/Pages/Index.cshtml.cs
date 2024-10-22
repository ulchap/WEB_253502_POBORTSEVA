using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253502_POBORTSEVA.Domain.Entities;
using WEB_253502_POBORTSEVA.UI.Services.ProductService;

namespace WEB_253502_POBORTSEVA.UI.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly int _itemsPerPage = 3;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public IList<Product> Product { get; set; } = default!;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public async Task<IActionResult> OnGetAsync(int pageNo = 1)
        {
            var res = await _productService.GetProductListAsync(null, pageNo);

            if (!res.Successfull)
                return NotFound(res.ErrorMessage ?? "");


            Product = res.Data?.Items!;
            CurrentPage = res.Data?.CurrentPage ?? 0;
            TotalPages = res.Data?.TotalPages ?? 0;

            return Page();

        }
    }
}
