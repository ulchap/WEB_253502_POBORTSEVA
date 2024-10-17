using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using WEB_253502_POBORTSEVA.API.Data;
using WEB_253502_POBORTSEVA.Domain.Entities;
using WEB_253502_POBORTSEVA.Domain.Models;
using WEB_253502_POBORTSEVA.API.Services.CategoryService;

namespace WEB_253502_POBORTSEVA.API.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly int _maxPageSize = 20;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public ProductService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        async public Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
            {
                pageSize = _maxPageSize;
            }

            var query = _context.Products
                                //.Include(c => c.Category)
                                .AsQueryable();

            var dataList = new ListModel<Product>();

            query = query.Where(p => categoryNormalizedName == null || p.Category.NormalizedName.Equals(categoryNormalizedName));

            // количество элементов в списке
            var count = await query.CountAsync(); //.Count();
            if (count == 0)
            {
                return ResponseData<ListModel<Product>>.Success(dataList);
            }

            // количество страниц
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (pageNo > totalPages)
                return ResponseData<ListModel<Product>>.Error("No such page");

            dataList.Items = await query
                .OrderBy(p => p.Id)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            //Console.WriteLine(dataList.Items[0].Category.ToString());
            dataList.CurrentPage = pageNo;
            dataList.TotalPages = totalPages;
            return ResponseData<ListModel<Product>>.Success(dataList);

        }

        async public Task<ResponseData<Product>> GetProductByIdAsync(int id)

        {
            var product = await _context.Products.FindAsync(id);
            if(product == null)
            {
                return ResponseData<Product>.Error("Product not found");
            }
            return ResponseData<Product>.Success(product);
        }
        async public Task UpdateProductAsync(int id, Product product)
        {
            var oldProduct = await _context.Products.FindAsync(id);
            if(oldProduct == null)
            {
                throw new Exception("Product not found");
            }
            oldProduct.Name = product.Name;
            oldProduct.Description = product.Description;
            oldProduct.Price = product.Price;
            oldProduct.ImagePath = product.ImagePath;
            oldProduct.Category = product.Category;

            _context.Products.Update(oldProduct);
            await _context.SaveChangesAsync();

        }
        async public Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        public async Task<ResponseData<Product>> CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return ResponseData<Product>.Success(product);
        }
        async public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return ResponseData<string>.Error("Product not found");
            }

            string imageRoot = Path.Combine(_configuration["AppUrl"], "Images");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + formFile.FileName;

            string imagePath = Path.Combine(imageRoot, uniqueFileName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            product.ImagePath = imagePath;
            await _context.SaveChangesAsync();

            return ResponseData<string>.Success(product.ImagePath);
        }
    }

}
