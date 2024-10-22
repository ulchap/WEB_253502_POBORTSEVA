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
    public class CreateModel : PageModel
    {
        private readonly IProductService _productservice;

        public CreateModel(IProductService productservice)
        {
            _productservice = productservice;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        [BindProperty]
        public IFormFile Image { get; set; }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var res = await _productservice.CreateProductAsync(Product, Image);

            if (!res.Successfull)
            {
                return Page();
            }

            //_context.Products.Add(Product);
            //await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
