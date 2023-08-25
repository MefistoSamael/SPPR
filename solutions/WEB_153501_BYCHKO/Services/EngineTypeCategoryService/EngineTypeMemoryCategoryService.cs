using WEB_153501_BYCHKO.Domain.Entities;
using WEB_153501_BYCHKO.Domain.Models;

namespace WEB_153501_BYCHKO.Services.EngineTypeCategoryService
{
    public class EngineTypeMemoryCategoryService : IEngineTypeCategoryService
    {
        public Task<ResponseData<List<EngineTypeCategory>>> GetCategoryListAsync()
        {
            var categories = new List<EngineTypeCategory> 
            {
                new EngineTypeCategory{ Id = "1", Name="турбовинтовой", NormalizedName="turboprop"},
                new EngineTypeCategory{ Id = "2", Name="поршневой", NormalizedName="reciprocating"},
                new EngineTypeCategory{ Id = "3", Name="реактивный", NormalizedName="propfan"},
                new EngineTypeCategory{ Id = "4", Name="турбовентиляторный", NormalizedName="turbofan"},
            };

            // создание объекта ответа и передача данных в него
            var result = new ResponseData<List<EngineTypeCategory>>();
            result.Data = categories;

            return Task.FromResult(result);
        }
    }
}
