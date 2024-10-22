using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253502_POBORTSEVA.Domain.Entities;
using WEB_253502_POBORTSEVA.UI.Services.ProductService;

namespace WEB_253502_POBORTSEVA.UI.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;

        public EditModel(IProductService productService)
        {
            _productService = productService;
        }


        [BindProperty]
        public Product Product { get; set; } = default!;

        [BindProperty]
        public IFormFile Image { get; set; }

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
            Product = res.Data!;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _productService.UpdateProductAsync(Product.Id, Product, Image);

            return RedirectToPage("./Index");
        }

        private async Task<bool> ProductExists(int id)
        {
            var res = await _productService.GetProductByIdAsync(id);
            return res.Successfull;
        }
    }
}
