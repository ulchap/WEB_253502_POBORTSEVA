using WEB_253502_POBORTSEVA.Domain.Entities;

namespace WEB_253502_POBORTSEVA.BlazorWASM.Services
{
    public interface IDataService
    {
        event Action DataLoaded;
        List<Category> Categories { get; set; }
        List<Product> Products { get; set; }
        bool Success { get; set; }
        string ErrorMessage { get; set; }
        int TotalPages { get; set; }
        int CurrentPage { get; set; }
        Category SelectedCategory { get; set; }

        Task GetProductListAsync(string? categoryNormalizedName, int pageNo = 1);
        Task GetCategoryListAsync();
    }

}
