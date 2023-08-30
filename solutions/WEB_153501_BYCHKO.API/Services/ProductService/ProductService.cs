using WEB_153501_BYCHKO.API.Data;
using WEB_153501_BYCHKO.Domain.Entities;
using WEB_153501_BYCHKO.Domain.Models;

namespace WEB_153501_BYCHKO.API.Services.ProductService
{
    public class ProductService : IProductService
    {
        AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        public Task<ResponseData<Airplane>> CreateProductAsync(Airplane product)
        {
            _context.Add(product);
            _context.SaveChanges();

            return Task.FromResult(new ResponseData<Airplane> { Data = product});
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Airplane>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<ListModel<Airplane>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductAsync(int id, Airplane product)
        {
            throw new NotImplementedException();
        }
    }
}
