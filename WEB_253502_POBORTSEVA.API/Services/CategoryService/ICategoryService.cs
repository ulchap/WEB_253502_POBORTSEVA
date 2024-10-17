using WEB_253502_POBORTSEVA.Domain.Models;
using WEB_253502_POBORTSEVA.Domain.Entities; 

namespace WEB_253502_POBORTSEVA.API.Services.CategoryService
{
    public interface ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}
