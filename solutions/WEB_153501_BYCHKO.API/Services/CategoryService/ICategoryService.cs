using WEB_153501_BYCHKO.Domain.Models;
using WEB_153501_BYCHKO.Domain.Entities;

namespace WEB_153501_BYCHKO.API.Services.CategoryService
{
    public interface ICategoryService
    {
        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        /// <returns></returns>
        public Task<ResponseData<List<EngineTypeCategory>>> GetCategoryListAsync();

    }
}
