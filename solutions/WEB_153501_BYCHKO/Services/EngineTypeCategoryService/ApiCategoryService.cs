using WEB_153501_BYCHKO.Domain.Entities;
using WEB_153501_BYCHKO.Domain.Models;

namespace WEB_153501_BYCHKO.Services.EngineTypeCategoryService
{
    public class ApiCategoryService : ICategoryService
    {
        public Task<ResponseData<List<EngineTypeCategory>>> GetCategoryListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
