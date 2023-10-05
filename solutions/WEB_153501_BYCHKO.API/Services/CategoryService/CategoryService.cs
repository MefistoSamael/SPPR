using Microsoft.EntityFrameworkCore;
using WEB_153501_BYCHKO.API.Data;
using WEB_153501_BYCHKO.Domain.Entities;
using WEB_153501_BYCHKO.Domain.Models;

namespace WEB_153501_BYCHKO.API.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        readonly AppDbContext _appDbContext;
        public CategoryService(AppDbContext db) 
        {
            _appDbContext = db;
        }
        public async Task<ResponseData<List<EngineTypeCategory>>> GetCategoryListAsync()
        {
            return new ResponseData<List<EngineTypeCategory>> { Data = await _appDbContext.engineTypes.ToListAsync() };
        }
    }
}
