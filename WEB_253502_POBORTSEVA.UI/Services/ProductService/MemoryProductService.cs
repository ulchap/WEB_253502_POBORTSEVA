using Microsoft.AspNetCore.Mvc;
using WEB_253502_POBORTSEVA.Domain.Entities;
using WEB_253502_POBORTSEVA.Domain.Models;
using WEB_253502_POBORTSEVA.UI.Services.CategoryService;

namespace WEB_253502_POBORTSEVA.UI.Services.ProductService
{
    //public class MemoryProductService : IProductService
    //{
    //    List<Product> _products;
    //    List<Category> _categories;
    //    int _itemsPerPage;

    //    public MemoryProductService([FromServices] IConfiguration config, ICategoryService categoryService)
    //    {
    //        _categories = categoryService.GetCategoryListAsync().Result.Data;
    //        _itemsPerPage = config.GetValue<int>("ItemsPerPage");
    //        SetupData();
    //    }
    //    public void SetupData()
    //    {
    //        _products = new List<Product>
    //        {
    //            new Product
    //            {
    //                Id = 1,
    //                Name="Smartphone",
    //                Description = "Latest model with advanced features",
    //                Price = 999,
    //                ImagePath = "Images/phone1.png",
    //                Category = _categories.Find(c=>c.NormalizedName.Equals("smartphones"))
    //            },
    //            new Product
    //            {
    //                Id=2,
    //                Name = "Laptop",
    //                Description = "High performance laptop",
    //                Price = 1499,
    //                ImagePath = "Images/laptop1.png",
    //                Category = _categories.Find(c=>c.NormalizedName.Equals("laptops"))
    //            },
    //            new Product
    //            {
    //                Id=3,
    //                Name = "Наушники",
    //                Description = "Быстрая зарядка и активное шумоподавление",
    //                Price = 299,
    //                ImagePath = "Images/headphones1.png",
    //                Category = _categories.Find(c=>c.NormalizedName.Equals("headphones"))
    //            },
    //            new Product
    //            {
    //                Id=4,
    //                Name = "Телевизор",
    //                Description = "Улучшенная цветопередача и тонкие грани",
    //                Price = 1199,
    //                ImagePath = "Images/tv1.png",
    //                Category = _categories.Find(c=>c.NormalizedName.Equals("tvs"))
    //            },
    //            new Product
    //            {
    //                Id=5,
    //                Name = "Пылесос",
    //                Description = "Обновлённый дизайн",
    //                Price = 799,
    //                ImagePath = "Images/vacum1.png",
    //                Category = _categories.Find(c=>c.NormalizedName.Equals("home-appliances"))
    //            },
    //        };
    //    }

    //    public Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
    //    {
    //        var filtredProducts = _products
    //            .Where(p => categoryNormalizedName == null || p.Category.NormalizedName.Equals(categoryNormalizedName))
    //            .ToList();

    //        int totalItems = filtredProducts.Count;
    //        var totalPages = (int)Math.Ceiling(totalItems / (double)_itemsPerPage);

    //        var items = filtredProducts
    //            .Skip((pageNo - 1) * _itemsPerPage)
    //            .Take(_itemsPerPage)
    //            .ToList();

    //        var listModel = new ListModel<Product>
    //        {
    //            Items = items,
    //            CurrentPage = pageNo,
    //            TotalPages = totalPages
    //        };

    //        return Task.FromResult(ResponseData<ListModel<Product>>.Success(listModel));
    //    }

    //    //public Task<ResponseData<Product>> GetProductByIdAsync(int id)
    //    //{

    //    //}
    //}
}
