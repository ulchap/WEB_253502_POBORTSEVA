using WEB_253502_POBORTSEVA.Domain.Entities;
using WEB_253502_POBORTSEVA.Domain.Models;

namespace WEB_253502_POBORTSEVA.UI.Services.CategoryService
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = new List<Category>
            {
                new Category {Id=1, Name="Смартфоны", NormalizedName="smartphones"},
                new Category {Id=2, Name="Ноутбуки", NormalizedName="laptops"},
                new Category {Id=3, Name="Телевизоры", NormalizedName="tvs"},
                new Category {Id=4, Name="Бытовая техника", NormalizedName="home-appliances"},
                new Category {Id=5, Name="Наушники", NormalizedName="headphones"},
                new Category {Id=6, Name="Аксессуары", NormalizedName="accessories"}
            };

            var result = ResponseData<List<Category>>.Success(categories);
            return Task.FromResult(result);
        }

    }
}
