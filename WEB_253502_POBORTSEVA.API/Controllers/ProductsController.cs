using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_253502_POBORTSEVA.API.Data;
using WEB_253502_POBORTSEVA.Domain.Entities;
using WEB_253502_POBORTSEVA.Domain.Models;
using WEB_253502_POBORTSEVA.API.Services.ProductService;
using Microsoft.AspNetCore.Authorization;

namespace WEB_253502_POBORTSEVA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Products
        [HttpGet("category/{category?}")]
        public async Task<ActionResult<ResponseData<List<Product>>>> GetProducts(string? category, int pageNo = 1, int pageSize = 3)
        {
            return Ok(await _productService.GetProductListAsync(category, pageNo, pageSize));
        }

        // GET: api/Products/5
        [Authorize(Policy = "admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseData<Product>>> GetProduct(int id)
        {
            var res = await _productService.GetProductByIdAsync(id);
            if (!res.Successfull)
            {
                return NotFound(res);
            }

            return Ok(res);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Policy = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseData<Product>>> PutProduct(int id, Product product)
        {
            try
            {
                await _productService.UpdateProductAsync(id, product);
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseData<Product>()
                {
                    Data = null,
                    Successfull = false,
                    ErrorMessage = ex.Message
                });
            }

            return Ok(new ResponseData<Product>()
            {
                Data = product
            });
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Policy = "admin")]
        [HttpPost]
        public async Task<ActionResult<ResponseData<Product>>> PostProduct(Product product)
        {
            var res = await _productService.CreateProductAsync(product);
            return res.Successfull ? Ok(res) : BadRequest(res);
        }

        // DELETE: api/Products/5
        [Authorize(Policy = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseData<Product>()
                {
                    Data = null,
                    Successfull = false,
                    ErrorMessage = ex.Message
                });
            }

            return NoContent();
        }

        private async Task<bool> ProductExists(int id)
        {
            return (await _productService.GetProductByIdAsync(id)).Successfull;
        }

    }
}
