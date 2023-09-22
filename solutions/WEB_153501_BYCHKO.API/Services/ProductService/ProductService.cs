using Microsoft.EntityFrameworkCore;
using WEB_153501_BYCHKO.API.Data;
using WEB_153501_BYCHKO.Domain.Entities;
using WEB_153501_BYCHKO.Domain.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WEB_153501_BYCHKO.API.Services.ProductService
{
    public class ProductService : IProductService
    {
        AppDbContext _context;
        private readonly int _maxPageSize;
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor accessor;

        public ProductService(AppDbContext context, ConfigurationManager configurationManager,
                            ConfigurationService configurationService, IWebHostEnvironment env,
                            IHttpContextAccessor accessor)
        {
            _context = context;

            _maxPageSize = Convert.ToInt32(configurationManager["MaxPageSize"]);
            this.env = env;
            this.accessor = accessor;

        }

        public async Task<ResponseData<Airplane>> CreateProductAsync(Airplane product)
        {
            // получаем существующую категорию
            // надо, потому что без этого блока, при передаче id категории в теле запроса
            // бросало исключение
            product.Category = _context.engineTypes.Where(c => c.NormalizedName == product.Category.NormalizedName).FirstOrDefault()!;

            if (product.Category == null)
                throw new Exception("невозможно найти категорию при добвалении товара");


            await _context.airplanes.AddAsync(product);

            _context.SaveChanges();

            return new ResponseData<Airplane> { Data = product};
        }

        public async Task DeleteProductAsync(int id)
        {
            var airplane = await _context.airplanes.FindAsync(id);
            
            if (airplane != null)
                _context.airplanes.Remove(airplane);

            _context.SaveChanges();
        }

        public async Task<ResponseData<Airplane>> GetProductByIdAsync(int id)
        {
            var query = _context.airplanes.AsQueryable();

           
            var data = await query.Where(p => p.Id == id).Include(p => p.Category).FirstOrDefaultAsync();
            var response = new ResponseData<Airplane>();

            if (data != null)
                 response.Data = data;
            else
            {
                response.ErrorMessage = "can't find such airplane";
                response.Success = false;
            }

            return response;
        }

        public async Task<ResponseData<ListModel<Airplane>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize= _maxPageSize;

            // объект ответа
            var response = new ResponseData<ListModel<Airplane>>();
            // данный которые засунутся в объект ответа
            ListModel<Airplane> listModel = new ListModel<Airplane>();

            var query = _context.airplanes.AsQueryable();
            // запрос для получения самолетов по категориям
            query = query
             .Where(d => categoryNormalizedName == null
             || d.Category.NormalizedName.Equals(categoryNormalizedName)).Include(p => p.Category);
            // количество элементов в списке
            var count = query.Count();

            if (count == 0)
            {
                response.ErrorMessage = "no airplanes founded";
                response.Success = false;
                response.Data = listModel;
                return response;
            }
            // проверка на допуститмость номера страницы
            // 
            // иф такой хитрый, чтобы учесть плюс одну страницу, если она не полностью заполнена
            if (pageNo * pageSize - count > pageSize)
            {
                throw new Exception("page number are greater then amount of pages");
            }

            listModel.Items = await query.Skip((pageNo - 1) * 3). // пропускаем элементы, которые не будут отображены
                Take(pageSize). // выбираем столько, сколько поместится на страницу
                ToListAsync(); // конвертируем в список


            // округляем в большую сторону чтобы поместились все элементы
            listModel.TotalPages = (int)Math.Ceiling((double)count / (double)pageSize);
            listModel.CurrentPage = pageNo;

            // если нет самолетов соответствующих данной категории
            // сообщаем об ошибке
            if (listModel.Items.Count == 0)
            {
                response.Success = false;
                response.ErrorMessage = "can't find airplanes with such engine type";
            }

            // заносим данные в объект ответа
            response.Data = listModel;
            return response;

        }

        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            // ищем самолет по id
            var responseData = await GetProductByIdAsync(id);
            Airplane airplane;

            if (!responseData.Success)
            {
                return new ResponseData<string>
                { Success = false, ErrorMessage = responseData.ErrorMessage };

            }
            else
                airplane = responseData.Data;

            var host = "https://" + accessor.HttpContext!.Request.Host;

            var imageFolder = Path.Combine(env.WebRootPath, "Images");


            if (formFile != null)
            {
                // удаляем предыдущее изображение
                if (!String.IsNullOrEmpty(airplane.PhotoPath))
                {
                    // получаем путь предыдущего изображения как путь к папке с изображеним +
                    // имя и расширение самого изображения
                    var prevImage = Path.Combine(imageFolder, Path.GetFileName(airplane.PhotoPath));
                    File.Delete(prevImage);

                    // или File.Delete(airplane.PhotoPath);
                }

                // Создать имя файла
                var ext = Path.GetExtension(formFile.FileName);
                var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);
                // получаем путь куда сохранять фото
                var filePath = Path.Combine(imageFolder, fName);

                // Сохранить файл
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                }

                airplane.PhotoPath = $"{host}/Images/{fName}";

                await _context.SaveChangesAsync();
                return new ResponseData<string>
                {
                    Data = airplane.PhotoPath
                };
            }
            else
            { return new ResponseData<string> { Success = false, ErrorMessage = "Error: no file where provided" }; }
        }

        // в методе update может возникнуть исключение при удалении
        // объекта в момент его изменения. Где его обрабатывать?
        public async Task UpdateProductAsync(int id, Airplane product)
        {
            // ищем самолет по id
            var responseData = await GetProductByIdAsync(id);
            Airplane airplane;

            if (responseData.Success)
            {
                airplane = responseData.Data;

                // наичнаем отслеживать найденный самолет и
                // переводим его в состояние Modified
                _context.airplanes.Update(airplane);

                airplane.Price = product.Price;
                airplane.Category = _context.engineTypes.Where(c => c.Id == product.Category.Id).First();
                airplane.PhotoPath = product.PhotoPath;
                airplane.Description = product.Description;
                airplane.Name = product.Name;

                // попробовать раскоментить эту штуку и закоментить все остальное
                //_context.Attach(airplane).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
        }
    }
}
