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
    public class DetailsModel : PageModel
    {
        private readonly IProductService _productService;

        public DetailsModel(IProductService productService)
        {
            _productService = productService;
        }

        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var res = await _productService.GetProductByIdAsync(id.Value);
            if (!res.Successfull)
            {
                return NotFound();
            }
            else
            {
                Product = res.Data!;
            }
            return Page();
        }
    }
}
