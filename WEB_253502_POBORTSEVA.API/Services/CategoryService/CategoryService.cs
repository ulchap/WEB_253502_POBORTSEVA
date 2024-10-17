using Microsoft.EntityFrameworkCore;
using WEB_253502_POBORTSEVA.API.Data;
using WEB_253502_POBORTSEVA.Domain.Entities;
using WEB_253502_POBORTSEVA.Domain.Models;

namespace WEB_253502_POBORTSEVA.API.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var data = await _context.Categories.ToListAsync();
            return ResponseData<List<Category>>.Success(data);
        }

    }
}
